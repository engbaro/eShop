using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eShop.Model
{
  public partial class Product
  {
    public Product()
    {
      this.OrderItems = new HashSet<OrderItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public float Price { get; set; }

    public int CategoryId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public virtual Category Category { get; set; }
  }
}
