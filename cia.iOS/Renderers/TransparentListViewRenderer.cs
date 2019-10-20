using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using UIKit;

using cia.iOS.Renderers;
using cia.Views;

[assembly: ExportRenderer(typeof(TransparentListView), typeof(TransparentListViewRenderer))]
namespace cia.iOS.Renderers
{

    public class TransparentListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;


            //remove the empty listview items
            Control.TableFooterView = new UIView();

            //TODO:the following lines are to set the delegate to set the background color of cellviews when the caching strategy is other than retain element
            //If we do this, then group headers will go away and won't be visible anymore
            //so if you want to use group headers on the list, then just use RetainElement for now.
            if (e.NewElement != null && ((TransparentListView)(e.NewElement)).CurrentCachingStrategy != ListViewCachingStrategy.RetainElement)
            {
                if (e.NewElement != null)
                {
                    Control.Delegate = new TransparentListviewDelegate((TransparentListView)e.NewElement);
                }
            }
        }

    }


    class TransparentListviewDelegate : UITableViewDelegate
    {
        private TransparentListView _listView;

        internal TransparentListviewDelegate(TransparentListView listView)
        {
            _listView = listView;
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, Foundation.NSIndexPath indexPath)
        {
            if (_listView != null && _listView.CellSelectedBackgroundColor != Color.Default)
            {
                cell.SelectedBackgroundView = new UIView
                {
                    BackgroundColor = _listView.CellSelectedBackgroundColor.ToUIColor()
                };
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _listView = null;
            }
            base.Dispose(disposing);
        }
    }
}
