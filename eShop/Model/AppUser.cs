using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eShop.Model
{


    public class AppUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public AppUser()
        {
            Orders = new HashSet<Order>();

            Addresses=new HashSet<Address>();
        }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(20)]
        public string FullName{ get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Postcode { get; set; }

        /*
    [Required]
    public string Phone { get; set; }

    [Required]
    public string Password { get; set; }



    [NotMapped]
    [Compare("Password", ErrorMessage = "confirm password shoild match the password!")]
    public string ConfirmPassword { get; set; }
    */
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<AppUser,int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}