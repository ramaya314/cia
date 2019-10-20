using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.Models;
using cia.ViewModels;
using cia.DataStores;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using PCLStorage;

namespace cia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartsPage : BackgroundImagePage
    {
        public CartsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = this;
            BackgroundImageSource = "patriotHacks.png";
            Title = "History";

            ToolbarItem addButton = new ToolbarItem
            {
                Text = "Add",
                //IconImageSource = DomaShared.RelativePathToFullPath(DynamicStyles.CommandCoinExitMenuItemIconImage, true),
                Order = ToolbarItemOrder.Primary,
                Command = new Command(async () => await OnAddClicked())
            };
            ToolbarItems.Add(addButton);
        }
        public CartsPage(IEnumerable<CartViewModel> carts) : this()
        {
            CellModelList = carts;
        }

        protected override async void OnAppearing()
        {

            var carts = await Warehouse.ShoppingCarts.GetAllAsync();
            var models = new List<CartViewModel>();
            foreach(var cart in carts)
            {
                models.Add(await cart.GetCellViewModel());
            }
            CellModelList = models;
            base.OnAppearing();
        }


        async Task OnAddClicked()
        {
            if (!(await RequestPermissions()))
            {
                await DisplayAlert("Permission Denied", "Camera permissions are needed to continue.", "Ok");
                return;
            }

            const string cameraAction = "Camera";
            const string storageAction = "Storage";


            MediaFile photo = null;

            if(CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsPickPhotoSupported && CrossMedia.Current.IsCameraAvailable)
            {
                var choice = await DisplayActionSheet("Choose Source", "Cancel", null, new string[] { cameraAction, storageAction });
                switch (choice)
                {
                    case cameraAction:
                        photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { CustomPhotoSize = 30 });
                        break;
                    case storageAction:
                        photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CustomPhotoSize = 30 });
                        break;
                    default:
                        return;
                }
            }
            else if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsCameraAvailable)
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { CustomPhotoSize = 30 });

            }
            else if (CrossMedia.Current.IsPickPhotoSupported)
            {
                photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CustomPhotoSize = 30 });
            }
            else
            {
                await DisplayAlert("Error", "No image capture is available on this device.", "Ok");
            }

            if (photo == null)
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
            PermissionStatus statusStorage = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status.Equals(PermissionStatus.Granted) && statusStorage.Equals(PermissionStatus.Granted))
                return true;

            var stati = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Camera, Permission.Storage });
            var cameraGranted = stati.Where(s => s.Key.Equals(Permission.Camera)).FirstOrDefault().Value.Equals(PermissionStatus.Granted);
            var storageGranted = stati.Where(s => s.Key.Equals(Permission.Camera)).FirstOrDefault().Value.Equals(PermissionStatus.Granted);
            return cameraGranted && storageGranted;
        }
        

        public IEnumerable<CartViewModel> CellModelList
        {
            get => (IEnumerable<CartViewModel>)GetValue(CellModelListProperty);
            set => SetValue(CellModelListProperty, value);
        }
        public readonly BindableProperty CellModelListProperty =
            BindableProperty.Create("CellModelList", typeof(IEnumerable<CartViewModel>), typeof(CartsPage), null);

        private async void MainListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Yield();
            await Task.Delay(100);
            var modelSender = (CartViewModel)((ListView)sender).SelectedItem;
            var page = new SummaryPage(modelSender.Cart, modelSender.ItemModels);
            //((ListView)sender).SelectedItem = null;
            await Task.Yield();
            await Task.Delay(200);
            try
            {
                await Navigation.PushAsync(page);
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
    }
}