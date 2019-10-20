
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using cia.Droid.Renderers;
using cia.Views;


[assembly: ExportRenderer(typeof(TransparentListView), typeof(TransparentListViewRenderer))]
namespace cia.Droid.Renderers
{

    public class TransparentListViewRenderer : ListViewRenderer
    {

        public TransparentListViewRenderer(Android.Content.Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                return;
            }

            Control.SetBackgroundColor(e.NewElement.BackgroundColor.ToAndroid());
        }
    }
}
