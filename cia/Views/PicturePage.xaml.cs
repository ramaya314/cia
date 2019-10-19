using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using PCLStorage;

using cia.Models;
using cia.Constants;

namespace cia.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PicturePage : ContentPage
	{
		public PicturePage ()
		{
			InitializeComponent ();
		}

        private async void OnCameraButtonClicked(object sender, EventArgs e)
        {
            if(!(await RequestPermissions()))
            {
                await DisplayAlert("Permission Denied", "Camera permissions are needed to continue.", "Ok");
                return;
            }

            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            if(photo == null)
            {
                await DisplayAlert("Error", "Could not save photo", "Ok");
                return;
            }

            ShoppingCart cart = new ShoppingCart
            {
                DateCreated = DateTime.Now.ToFileTimeUtc()
            };
            
            IFile photoFile = await FileSystem.Current.LocalStorage.CreateFileAsync(cart.ReceiptImageFilePath, CreationCollisionOption.ReplaceExisting);
            using (Stream writeStream = await photoFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
            using (Stream readStream = photo.GetStream())
            {
                await readStream.CopyToAsync(writeStream);
            }

            var photoSource = ImageSource.FromStream(() => { return photo.GetStream(); });

            var analyzingPage = new AnalyzingPage(cart, photoSource);
            await Navigation.PushAsync(analyzingPage);
        }

        /// <summary>
        /// Return a bool representing wether we have camera permissions
        /// </summary>
        /// <returns></returns>
        private async Task<bool> RequestPermissions()
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (status.Equals(PermissionStatus.Granted))
                return true;

            var stati = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Camera });
            return stati.Where(s => s.Key.Equals(Permission.Camera)).FirstOrDefault().Value.Equals(PermissionStatus.Granted);
        }
        

    }
}