using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eShop.Model
{
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<AppUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(FirdoosModel context)
            : base(context)
        {
        }

        public virtual Boolean FindByPhoneNumberOrEmail(string phoneNumber,string email)
        {
            var user = Users.FirstOrDefault(u => (u.PhoneNumber == phoneNumber) ||(u.Email==email));
            if (user != null)
            { return true;}
            else
            {
                return false;
            }
        }

    }

   

     



    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(FirdoosModel context)
            : base(context)
        {
        }
    }
}