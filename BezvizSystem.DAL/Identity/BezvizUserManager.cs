using BezvizSystem.DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Identity
{
    public class BezvizUserManager : UserManager<BezvizUser>
    {
        public BezvizUserManager(IUserStore<BezvizUser> store)
            : base(store)
        {

            this.EmailService = new EmailService();

        }

        public static BezvizUserManager Create(IdentityFactoryOptions<BezvizUserManager> options,
                                                       IOwinContext context)
        {
            BezvizContext db = context.Get<BezvizContext>();
            var manager = new BezvizUserManager(new UserStore<BezvizUser>(db));

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<BezvizUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            manager.EmailService = new EmailService();
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var from = "egorik-555@yandex.ru";
            var pass = "26071987Egor";

            SmtpClient client = new SmtpClient("smtp.yandex.ru", 25);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;

            var mail = new MailMessage(from, message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);
        }
    }
}
