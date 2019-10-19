using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cia.Models;

namespace cia.DataStores
{
    public class ItemDataStore : BaseDataStore<Item>
    {
        public ItemDataStore()
        {
        }

        public override async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await db.Table<Item>().ToListAsync();
        }

        public override async Task<Item> GetAsync(int id)
        {
            return await db.Table<Item>().Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<Item> GetMatchingItem(Item item)
        {
            return await db.Table<Item>().Where(i => i.Equals(item)).FirstOrDefaultAsync();
        }
    }
}