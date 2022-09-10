using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.ProductCategory
{
    public class CategoryInfoViewModel
    {
        public int CategoryIDLevel3 { get; set; }
        public string CategoryTitleLevel3 { get; set; }
        public string CategoryURL { get; set; }

        public string CategoryTitleLevel1 { get; set; }
        public int? CategoryIDLevel1 { get; set; }

        public string CategoryTitleLevel2 { get; set; }
        public int? CategoryIDLevel2 { get; set; }       
    }
}
