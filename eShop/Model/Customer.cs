using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eShop.Model
{

  public partial class Customer
  {
    public Customer()
    {
      Orders = new HashSet<Order>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }

        [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Postcode { get; set; }

    [NotMapped]
    [Compare("Password", ErrorMessage = "confirm password shoild match the password!")]
    public string ConfirmPassword { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
  }
}
