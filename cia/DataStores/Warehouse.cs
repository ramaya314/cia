
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using cia.Models;
using cia.DataStores;

namespace cia.DataStores
{
    public static class Warehouse
    {
        public static IDataStore<ShoppingCartItem> ShoppingCartItems => new ShoppingCartItemDataStore();
        public static IDataStore<ShoppingCart> ShoppingCarts => new ShoppingCartDataStore();
        public static IDataStore<Item> Items => new ItemDataStore();

    }
}
