﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm.Entity
{
    [Serializable]
    public class Product : EntityBase
    {
        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }
        
        public DateTime CreateTime { get; set; }
    }
}
