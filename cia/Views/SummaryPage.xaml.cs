using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.Models;
using cia.ViewModels;

namespace cia.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SummaryPage : ContentPage
	{
        ShoppingCart _cart;

		public SummaryPage (ShoppingCart cart, IEnumerable<SummaryCellViewModel> models)
		{
			InitializeComponent ();
            _cart = cart;
            CellModelList = models;

            NavigationPage.SetHasBackButton(this, false);
            BindingContext = this;
		}


        public IEnumerable<SummaryCellViewModel> CellModelList
        {
            get => (IEnumerable<SummaryCellViewModel>)GetValue(CellModelListProperty);
            set => SetValue(CellModelListProperty, value);
        }
        public readonly BindableProperty CellModelListProperty =
            BindableProperty.Create("CellModelList", typeof(IEnumerable<SummaryCellViewModel>), typeof(SummaryPage), null);

        private void MainListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void SummaryViewCell_OnBetterAlternativeButtonClicked(object sender, EventArgs e)
        {

        }
    }
}