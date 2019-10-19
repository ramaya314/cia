using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cia.Models;

namespace cia.DataStores
{
    public class ShoppingCartItemDataStore : BaseDataStore<ShoppingCartItem>
    {

        #region base overrides
        public override async Task<IEnumerable<ShoppingCartItem>> GetAllAsync()
        {
            return await db.Table<ShoppingCartItem>().ToListAsync();
        }

        public override async Task<ShoppingCartItem> GetAsync(int id)
        {
            return await db.Table<ShoppingCartItem>().Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<ShoppingCartItem> GetMatchingItem(ShoppingCartItem item)
        {
            return await db.Table<ShoppingCartItem>().Where(i => i.Equals(item)).FirstOrDefaultAsync();
        }
        #endregion baseoverrides

        public async Task<IEnumerable<ShoppingCartItem>> GetAllItemsByCart(ShoppingCart cart)
        {
            if (cart == null)
                return new List<ShoppingCartItem>();

            return await db.Table<ShoppingCartItem>().Where(sci => sci.ShoppingCartId == cart.Id).ToListAsync();
        }

    }
}