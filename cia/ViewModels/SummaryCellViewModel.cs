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

        public string Name => Item.Name;

        public float TotalPrice => Item.Price * Amount;

        public float TotalCarbon => Item.Co2 * Amount;
    }
}
