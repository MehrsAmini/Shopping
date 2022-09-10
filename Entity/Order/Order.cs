using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity.Order
{
    public class Order
    {
        #region property
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int OrderSum { get; set; }

        public bool IsFinaly { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        #endregion

        #region relations
        public virtual User User { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
