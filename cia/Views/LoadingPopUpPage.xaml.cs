using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

namespace cia.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopUpPage : PopupPage
    {
        public LoadingPopUpPage()
        {
            InitializeComponent();

            CloseWhenBackgroundIsClicked = false;
        }

        public async Task DismissAsync()
        {
            try
            {
                await Navigation.PopPopupAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dismiss()
        {
            try
            {
                Navigation.PopPopupAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
