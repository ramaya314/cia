
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
        public static ShoppingCartItemDataStore ShoppingCartItems => new ShoppingCartItemDataStore();
        public static ShoppingCartDataStore ShoppingCarts => new ShoppingCartDataStore();
        public static ItemDataStore Items => new ItemDataStore();

    }
}
