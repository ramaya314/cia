using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.ViewModels;

namespace cia.Views.Cells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SummaryViewCell : ViewCell
	{
        public event EventHandler OnBetterAlternativeButtonClicked;

        public SummaryViewCell ()
		{
			InitializeComponent ();
		}

        private void DoOnBetterAlternativeButtonClicked(object sender, EventArgs e)
        {
            OnBetterAlternativeButtonClicked?.Invoke((SummaryCellViewModel)BindingContext, e);
        }
    }
}