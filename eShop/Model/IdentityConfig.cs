using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Twilio;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using eShop.Model;
using Twilio.Clients;
using Twilio.Http;
using Twilio.Jwt.Client;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;



namespace eShop.Model
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Find your Account Sid and Auth Token at twilio.com/console
            const string accountSid = "AC9af3cb7a99db1590de56feda151f1528";
            const string authToken = "your_auth_token";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber("+15017250604");
            var messages = MessageResource.Create(
                to,
                from: new PhoneNumber("+15558675309"),
                body: "This is the ship that made the Kessel Run in fourteen parsecs?");

          //  TwilioRestClient client = new TwilioRestClient("<Your Account SID>", "<Your account auth token>");
           
            //client.SendSmsMessage("<The number you are sending from>", message.Destination, message.Body);
            
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    
    // Configure the application sign-in manager which is used in this application.
    public class AppSignInManager : SignInManager<AppUser, int>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }


  

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }
    
    public class AppUserManager : UserManager<AppUser, int>
    {
        private static FirdoosModel db= new FirdoosModel();
        private CustomUserStore _userStore=new CustomUserStore(db);
        public AppUserManager(CustomUserStore store)
            : base(store)
        {
        }
        public CustomUserStore UserStore
        {

            get
            {
                return _userStore;
            }
            private set
            {
                _userStore = value;
            }
        }
        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new CustomUserStore(context.Get<FirdoosModel>()));
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<AppUser, int>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone 
            // and Emails as a step of receiving a code for verifying the user 
            // You can write your own provider and plug in here. 
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<AppUser, int> {
                    MessageFormat = "Your security code is: {0}"
                });
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<AppUser, int> {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<AppUser, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }//end of create

        public CustomUserStore NewStore { get; set; }

        public virtual Boolean FindByPhoneNumberUserManager(string phoneNumber,String email)
        {
           
            if (phoneNumber == null ||email==null )
            {
                throw new ArgumentNullException("phoneNumberOremail");
            }
            Boolean value =UserStore.FindByPhoneNumberOrEmail(phoneNumber,email);
            return value;
        }
    }
}
