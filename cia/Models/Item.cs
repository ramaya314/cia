using System;

namespace cia.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        

        public float Co2 { get; set; }

        public float Price { get; set; }
    }
}