using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using cia.DataStores;
using cia.Views;
using System.Threading.Tasks;

using cia.Utils;
using PCLStorage;

namespace cia
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ItemDataStore>();
            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await DataAccess.InitDatabase();
            var instance = DataAccessAsync.Instance;
            Console.WriteLine("DB PAth: " + instance.DatabasePath);

            await TestMyMethod();
        }

        private async Task TestMyMethod()
        {

            var dummy = new AnalyzingPage(new Models.ShoppingCart(), ImageSource.FromFile("balsh"));

            var path = Path.Combine(FileSystem.Current.LocalStorage.Path, "test/testreceiptImage.png");
            var file = await EmbeddedResourceManager.CopyFileToLocation("receipt.png", path, true);
            await dummy.GetItemsFromImage(path);
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
