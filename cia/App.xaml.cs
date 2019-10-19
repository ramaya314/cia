using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using cia.DataStores;
using cia.Views;

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
