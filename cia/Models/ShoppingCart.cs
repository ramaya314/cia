using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;

using PCLStorage;

using cia.Constants;
using cia.DataStores;

namespace cia.Models
{
    public class ShoppingCart : BaseModel
    {
        public string Name { get; set; }

        public long DateCreated { get; set; }


        #region pure getters

        public string ReceiptImageFilePath => Path.Combine(FileSystem.Current.LocalStorage.Path, Paths.ReceiptImagesFolder, Id.ToString());

        #endregion pure getters

        #region query functions
        public async Task<IEnumerable<ShoppingCartItem>> GetAllCartItems()
        {
            return await Warehouse.ShoppingCartItems.GetAllItemsByCart(this);
        }
        #endregion query functions
    }
}
