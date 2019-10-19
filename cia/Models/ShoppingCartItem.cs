using System;
using System.Collections.Generic;
using System.Text;

namespace cia.Models
{
    public class ShoppingCartItem : BaseModel
    {
        public int ShoppingCartId { get; set; }
        public int ItemId { get; set; }
        public float Ammount { get; set; }
    }
}
