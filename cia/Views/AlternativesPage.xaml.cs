using System;
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
	public partial class AlternativesPage : BackgroundImagePage
    {
		public AlternativesPage (IEnumerable<SummaryCellViewModel> models)
		{
			InitializeComponent ();

            CellModelList = models;

            

            BindingContext = this;
            BackgroundImageSource = "patriotHacks.png";
            Title = "Alterntive Choices";
        }



        public IEnumerable<SummaryCellViewModel> CellModelList
        {
            get => (IEnumerable<SummaryCellViewModel>)GetValue(CellModelListProperty);
            set => SetValue(CellModelListProperty, value);
        }
        public readonly BindableProperty CellModelListProperty =
            BindableProperty.Create("CellModelList", typeof(IEnumerable<SummaryCellViewModel>), typeof(SummaryPage), null);

    }
}