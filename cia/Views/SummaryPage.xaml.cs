using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.Models;

namespace cia.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SummaryPage : ContentPage
	{
        ShoppingCart _cart;
        IEnumerable<ShoppingCartItem> _cartItems;

		public SummaryPage (ShoppingCart cart, IEnumerable<ShoppingCartItem> cartItems)
		{
			InitializeComponent ();
            _cart = cart;
            _cartItems = cartItems;

            NavigationPage.SetHasBackButton(this, false);
		}
	}
}