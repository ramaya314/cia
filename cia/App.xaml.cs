using System;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using cia.DataStores;
using cia.Views;
using System.Threading.Tasks;
using System.Collections.Generic;

using cia.Utils;
using cia.DataStores;
using cia.ViewModels;
using cia.Views;
using PCLStorage;

namespace cia
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ItemDataStore>();
            MainPage = new DreamNavigationPage(new CartsPage());
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await DataAccess.InitDatabase();
            var instance = DataAccessAsync.Instance;
            Console.WriteLine("DB PAth: " + instance.DatabasePath);

            //await TestMyMethod();
            //await LoadTestSummaryPage();
        }

        private async Task TestMyMethod()
        {

            var dummy = new AnalyzingPage(new Models.ShoppingCart(), ImageSource.FromFile("balsh"));

            var path = Path.Combine(FileSystem.Current.LocalStorage.Path, "test/testreceiptImage.png");
            var file = await EmbeddedResourceManager.CopyFileToLocation("receipt.png", path, true);
            await dummy.GetItemsFromImage(path);
        }

        private async Task LoadTestSummaryPage()
        {
            var firstShoppingCart = (await Warehouse.ShoppingCarts.GetAllAsync()).FirstOrDefault();
            var cartItems = await firstShoppingCart.GetAllCartItems();

            var models = new List<SummaryCellViewModel>();
            foreach(var cartItem in cartItems)
            {
                models.Add(await cartItem.GetSummaryCellViewModel());
            }

            var dummy = new SummaryPage(firstShoppingCart, models);
            MainPage = new DreamNavigationPage(dummy);
        }

        private async Task LoadTestPicturePage()
        {

        }
        

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
