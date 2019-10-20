using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.Style;

namespace cia.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();


            var gamePage = new DreamNavigationPage(new PicturePage
            {
                BackgroundImageSource = "patriotHacks",
                Title = "New Cart"
            });
            gamePage.Title = "New Cart";
            //gamePage.IconImageSource = "donutTabIcon";
            Children.Add(gamePage);

            //var aboutDreamersPage = new DreamNavigationPage(new AboutDreamersPage
            //{
            //    BackgroundImageSource = "butterfly.jpg"
            //});
            //aboutDreamersPage.Title = "About Mason DREAMers";
            //aboutDreamersPage.Icon = "dreamersTabIcon";
            //Children.Add(aboutDreamersPage);

            //var aboutAppPage = new DreamNavigationPage(new AboutAppPage
            //{
            //    BackgroundImageSource = "butterfly.jpg"
            //});
            //aboutAppPage.Title = "About App";
            //aboutAppPage.Icon = "aboutTabIcon";
            //Children.Add(aboutAppPage);


            BarBackgroundColor = ColorPalette.PrimaryColor;
            BarTextColor = ColorPalette.SecondaryColor;
        }
    }
}