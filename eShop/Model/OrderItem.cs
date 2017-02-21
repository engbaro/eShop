using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Model
{

  public partial class OrderItem
  {
   [Key]
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public double Price { get; set; }
    public int Quantity { get; set; }

    public virtual Product Product { get; set; }
    public virtual Order Order { get; set; }
  }
}
