﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm
{
    public class PageCriteria
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Fields { get; set; }


        public string Table { get; set; }

        public string Where { get; set; }

        public object Parameter { get; set; }

        public string Order { get; set; }
        
    }
}
