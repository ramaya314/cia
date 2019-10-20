using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using cia.Style;

namespace cia.Views
{
    public class DreamNavigationPage : NavigationPage
    {
        public DreamNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = ColorPalette.PrimaryColor;
            BarTextColor = ColorPalette.SecondaryColor;
        }
    }
}
