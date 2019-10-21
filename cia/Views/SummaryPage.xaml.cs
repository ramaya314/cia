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
	public partial class SummaryPage : BackgroundImagePage
	{
        ShoppingCart _cart;

		public SummaryPage (ShoppingCart cart, IEnumerable<SummaryCellViewModel> models)
		{
            _cart = cart;
            CellModelList = models;

            //NavigationPage.SetHasBackButton(this, false);
            BindingContext = this;
            BackgroundImageSource = "patriotHacks.png";
            Title = "Receipt";
            
			InitializeComponent ();
        }


        public IEnumerable<SummaryCellViewModel> CellModelList
        {
            get => (IEnumerable<SummaryCellViewModel>)GetValue(CellModelListProperty);
            set => SetValue(CellModelListProperty, value);
        }
        public readonly BindableProperty CellModelListProperty =
            BindableProperty.Create("CellModelList", typeof(IEnumerable<SummaryCellViewModel>), typeof(SummaryPage), null);

        private async void MainListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Yield();
            await Task.Delay(100);
            var modelSender = (SummaryCellViewModel)((ListView)sender).SelectedItem;
            //var page = new SummaryPage(modelSender.Cart, modelSender.ItemModels);
            //((ListView)sender).SelectedItem = null;
            await Task.Yield();
            await Task.Delay(200);
            try
            {
                //await Navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void SummaryViewCell_OnBetterAlternativeButtonClicked(object sender, EventArgs e)
        {

        }
    }
}