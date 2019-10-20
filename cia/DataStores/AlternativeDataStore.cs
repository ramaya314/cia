using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using cia.Models;

namespace cia.DataStores
{
    public class AlternativeDataStore : BaseDataStore<Alternative>
    {

        #region base overrides
        public override async Task<IEnumerable<Alternative>> GetAllAsync()
        {
            return await db.Table<Alternative>().ToListAsync();
        }

        public override async Task<Alternative> GetAsync(int id)
        {
            return await db.Table<Alternative>().Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<Alternative> GetMatchingItem(Alternative item)
        {
            return await db.Table<Alternative>().Where(i => i.Equals(item)).FirstOrDefaultAsync();
        }
        #endregion baseoverrides

    }
}
