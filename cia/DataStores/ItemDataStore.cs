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

        public async Task<Item> GetByNameAsync(string name)
        {
            return await db.Table<Item>().Where(item => item.Name == name).FirstOrDefaultAsync();
        }

        public override async Task<Item> GetMatchingItem(Item item)
        {
            return await db.Table<Item>().Where(i => i.Equals(item)).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Item>> GetAlternativesForItem(Item item)
        {
            string query = "SELECT * FROM Item i " +
                           "JOIN Alternative a ON a.RightId = i.Id " +
                           $"WHERE a.LeftId = {item.Id}";
            var results = await db.QueryAsync<Item>(query);
            return results;
        }
    }
}