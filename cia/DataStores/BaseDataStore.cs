using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cia.Models;

namespace cia.DataStores
{
    public class BaseDataStore<T> : IDataStore<T> where T : BaseModel
    {
        readonly List<T> Store;

        public async Task<int> AddAsync(T item)
        {
            await Task.Yield();

            if(item.Id < 0 && !(await Exists(item)))
            {
                int newId = NextValidId;
                item.Id = newId;
                Store.Add(item);
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
            var oldItem = Store.Where((T arg) => arg.Id == item.Id).FirstOrDefault();
            Store.Remove(oldItem);
            Store.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var oldItem = Store.Where((T arg) => arg.Id == id).FirstOrDefault();
            Store.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<T> GetAsync(int id)
        {
            return await Task.FromResult(Store.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Store);
        }

        public int NextValidId => Store.Max(i => i.Id) + 1;

        public async Task<bool> Exists(T item)
        {
            return await Task.FromResult(Store.Contains(item));
        }
    }
}
