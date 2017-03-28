using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using eShop.Settings;

namespace eShop.Model
{
  public class FirdoosModel : IdentityDbContext<AppUser,CustomRole,int,CustomUserLogin,CustomUserRole,CustomUserClaim>
  {
    public FirdoosModel() : base(ConnectionStrings.ConnectionString)
    {
            Database.SetInitializer<FirdoosModel>(null);
    }

        public static FirdoosModel Create()
        {
            return new FirdoosModel();
        }
        public DbSet<CustomUserLogin> CustomUserLogins { get; set; }
        public DbSet<CustomUserClaim> CustomUserClaims { get; set; }
        public DbSet<CustomUserRole> CustomUserRoles { get; set; }
        public virtual DbSet<Address> Adresses { get; set; }

        public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
        
    }
}