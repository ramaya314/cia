using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using cia.ViewModels;
using cia.DataStores;

namespace cia.Models
{
    public class ShoppingCartItem : BaseModel
    {
        public int ShoppingCartId { get; set; }
        public int ItemId { get; set; }
        public float Amount { get; set; }

        public async Task<SummaryCellViewModel> GetSummaryCellViewModel()
        {
            var item = await GetItem();
            var model = new SummaryCellViewModel
            {
                Item = item,
                Alternatives = await Warehouse.Items.GetAlternativesForItem(item),
                Amount = Amount
            };
            return model;
        }

        public async Task<Item> GetItem()
        {
            return await Warehouse.Items.GetAsync(ItemId);
        }
    }
}
