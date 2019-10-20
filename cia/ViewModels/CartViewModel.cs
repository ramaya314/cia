
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using cia.Models;

namespace cia.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        public ShoppingCart Cart { get; set; }

        public string DisplayDate => Cart.DisplayDate;

        public IEnumerable<SummaryCellViewModel> ItemModels { get; set; }

        public string Name => $"{Cart.Name}";

        public string TotalPrice => ItemModels.Sum(im => im.TotalPriceDecimal).ToString("C2");

        public float TotalCarbon => ItemModels.Sum(im => im.TotalCarbon);
    }
}

