using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.Models;
using cia.Services;

namespace cia.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AnalyzingPage : ContentPage
	{

        public IDataStore<ShoppingCartItem> ShoppingCartItemDataStore => DependencyService.Get<IDataStore<ShoppingCartItem>>();

        private ShoppingCart _cart;

		public AnalyzingPage (ShoppingCart cart, ImageSource receiptImageSource)
		{
			InitializeComponent();
            _cart = cart;

            if(receiptImageSource != null)
                ReceiptImage.Source = receiptImageSource;

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var cartItems = await GetItemsFromImage(_cart.ReceiptImageFilePath);
            foreach(var cartItem in cartItems)
            {
                await ShoppingCartItemDataStore.AddItemAsync(cartItem);
            }

            var summaryPage = new SummaryPage(_cart, cartItems);
            await Navigation.PushAsync(summaryPage);
        }

        /// <summary>
        /// This is the meat and bones of the parsing algorithm.
        /// This should take the image path, open up the image and send it to the parsing service that we will be using
        /// then with the information reteived asynchronously from the parsing service, match all of the items to an <see cref="Item"/> object
        /// then grab all items and return them
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private async Task<List<ShoppingCartItem>> GetItemsFromImage(string imagePath)
        {
            List<ShoppingCartItem> cartItems = new List<ShoppingCartItem>();

            //TODO:

            return cartItems;
        }
    }
}