using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;

using PCLStorage;

using cia.Constants;
using cia.DataStores;
using cia.ViewModels;

namespace cia.Models
{
    public class ShoppingCart : BaseModel
    {
        public string Name { get; set; }

        public long DateCreated { get; set; }


        #region pure getters

        public string ReceiptImageFilePath => Path.Combine(FileSystem.Current.LocalStorage.Path, Paths.ReceiptImagesFolder, Id.ToString());
        
        public string DisplayDate => (UnixTimeStampToDateTime(DateCreated)).ToString("f");


        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        #endregion pure getters

        #region query functions
        public async Task<IEnumerable<ShoppingCartItem>> GetAllCartItems()
        {
            return await Warehouse.ShoppingCartItems.GetAllItemsByCart(this);
        }

        public async Task<CartViewModel> GetCellViewModel()
        {
            var cartItems = await GetAllCartItems();
            var models = new List<SummaryCellViewModel>();
            foreach (var cartItem in cartItems)
            {
                models.Add(await cartItem.GetSummaryCellViewModel());
            }
            return new CartViewModel
            {
                Cart = this,
                ItemModels = models
            };
        }
        #endregion query functions
    }
}
