using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using cia.Models;

namespace cia.DataStores
{
    public abstract class BaseDataStore<T> : IDataStore<T> where T : BaseModel
    {
        protected static DataAccessAsync db => DataAccessAsync.Instance;

        public abstract Task<T> GetMatchingItem(T item);
        
        public abstract Task<IEnumerable<T>> GetAllAsync();

        public abstract Task<T> GetAsync(int id);

        public async Task<int> AddAsync(T item)
        {
            if(item.Id < 0 && !(await Exists(item)))
            {
                int newId = await db.InsertAsync(item);
                return newId;
            }
            else
            {
                await UpdateAsync(item);
                return item.Id;
            }
        }

        public async Task<bool> UpdateAsync(T item)
        {
            var oldItem = await GetMatchingItem(item);
            if(oldItem != null)
            {
                await db.UpdateAsync(item);
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var oldItem = await GetAsync(id);
            if (oldItem != null)
            {
                await db.DeleteAsync(oldItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        

        public async Task<bool> Exists(T item)
        {
            return await GetMatchingItem(item) != null;
        }
    }
}
