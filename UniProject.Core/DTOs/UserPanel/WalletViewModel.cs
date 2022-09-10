using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.UserPanel
{
    public class WalletViewModel
    {
        public int Amount { get; set; }
        public int Type { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
    }
}
