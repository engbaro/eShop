using System;
using System.Collections.Generic;

namespace eShop.Model
{

  public partial class Order
  {
    public Order()
    {
      OrderItems = new HashSet<OrderItem>();
    }

    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Address { get; set; }

    public string Town { get; set; }

    public string Country { get; set; }

    public string Postcode { get; set; }

    public string Phone { get; set; }

    public double TotalPrice { get; set; }

    public string CustomerName { get; set; }

    public double DeliveryCost { get; set; }

    public string Notes { get; set; }

    public int CompanyId { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }
  }
}
