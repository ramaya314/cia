
using System;
using Xamarin.Forms;

namespace cia.Views
{
    public class TransparentListView : ListView
    {

        ListViewCachingStrategy _cachingStrategy = ListViewCachingStrategy.RetainElement;
        public ListViewCachingStrategy CurrentCachingStrategy { get => _cachingStrategy; }

        public TransparentListView() : base() { }
        public TransparentListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            _cachingStrategy = strategy;
        }


        /// <summary>
        /// When using ListViewCachingStrategy=RecycleElement, the cells themselves won't be able to set a background color
        /// when selected so we need to do it at the listview level through the renderers.
        /// NOTE://this only works on iOS. For Android there is no such solution and the android:colorActivatedHighlight xml style must be set instead
        /// </summary>
        /// <value>The color of the selected background.</value>
        public Color CellSelectedBackgroundColor
        {
            get { return (Color)GetValue(CellSelectedBackgroundColorProperty); }
            set { SetValue(CellSelectedBackgroundColorProperty, value); }
        }
        public static readonly BindableProperty CellSelectedBackgroundColorProperty =
            BindableProperty.Create("CellSelectedBackgroundColor",
                                    typeof(Color),
                                    typeof(TransparentListView),
                                    Color.Default);

    }
}
