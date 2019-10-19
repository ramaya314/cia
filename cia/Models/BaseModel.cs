using System;
using System.Collections.Generic;
using System.Text;

namespace cia.Models
{
    public abstract class BaseModel
    {
        public BaseModel() { }
        public int Id { get; set; } = -1;
    }
}
