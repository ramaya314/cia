using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cia.Models
{
    public class Item : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public float Co2 { get; set; }
        public float Price { get; set; }


    }
}