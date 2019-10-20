using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cia.Models;

namespace cia.DataStores
{
    public class ShoppingCartDataStore : BaseDataStore<ShoppingCart>
    {

        public override async Task<IEnumerable<ShoppingCart>> GetAllAsync()
        {
            var res = await db.Table<ShoppingCart>().ToListAsync();
            return res;
        }

        public override async Task<ShoppingCart> GetAsync(int id)
        {
            return await db.Table<ShoppingCart>().Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<ShoppingCart> GetMatchingItem(ShoppingCart item)
        {
            return await db.Table<ShoppingCart>().Where(i => i.Equals(item)).FirstOrDefaultAsync();
        }
    }
}