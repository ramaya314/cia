using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using cia.Models;

namespace cia.ViewModels
{
    public class SummaryCellViewModel : BaseViewModel
    {
        public float Amount { get; set; }
        public Item Item { get; set; }

        public IEnumerable<Item> Alternatives { get; set; }

        public bool HasBetterAlternatives => Alternatives.Any(a => a.Co2 < Item.Co2);

        public string Name => $"{Amount} x {Item.Name}";

        public decimal TotalPriceDecimal => Convert.ToDecimal((Item.Price * Amount));
        public string TotalPrice => TotalPriceDecimal.ToString("C2");

        public float TotalCarbon => Item.Co2 * Amount;
    }
}
