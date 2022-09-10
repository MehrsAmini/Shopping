using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.Product
{
    public class AddNewFeaturesDto
    {
        [Display(Name = "عنوان")]
        public string FeatuerTitle { get; set; }

        [Display(Name = "مقدار")]
        public string FeatureValue { get; set; }
    }
}
