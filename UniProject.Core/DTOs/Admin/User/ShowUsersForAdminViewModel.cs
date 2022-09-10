using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.DataLayer.Entity;

namespace UniProject.Core.DTOs.Admin
{
    public class ShowUsersForAdminViewModel
    {
        public List<User> Users { get; set; }
        public int WalletBalance { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
