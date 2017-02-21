using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Model
{

  public partial class Order
  {
    public Order()
    {
      OrderItems = new HashSet<OrderItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public string Address { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public double DeliveryCost { get; set; }
        [Required]
        public string Notes { get; set; }
        [Required]
        public int CompanyId { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }
  }
}
