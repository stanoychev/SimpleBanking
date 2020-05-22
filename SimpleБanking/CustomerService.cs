using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBanking
{
    public interface ICustomerService
    {
        (string name, string cookie) Login((string user, string pin) credentials);
        void Logout(string cookie);
        bool CustomerExists(string user);
        bool IsLoggedIn(string cookie);
        bool IsExpired(string cookie);
    }

    public class CustomerService : ICustomerService
    {
        const double sessionTimeInMinutes = 10d;
        readonly IBankDb bankDb;
        readonly ICookieManager cookieManager;
        readonly Func<string, bool> isNullEmpryOrWhiteSpace = x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x);

        public CustomerService(IBankDb bankDb_, ICookieManager cookieManager_)
        {
            bankDb = bankDb_;
            cookieManager = cookieManager_;
        }

        public (string name, string cookie) Login((string user, string pin) credentials)
        {
            if (isNullEmpryOrWhiteSpace(credentials.user) || isNullEmpryOrWhiteSpace(credentials.pin))
                throw new ArgumentNullException("User or pin cannot be null.");

            var customer = GetCustomer(credentials);
            if (customer == null)
                return default;

            var cookie = Guid.NewGuid().ToString();
            cookieManager.SaveCookie(cookie, customer.Id);

            customer.Cookie = cookie;
            customer.ExpiresOn = DateTime.Now.AddMinutes(sessionTimeInMinutes);
            bankDb.SaveChanges();

            return (customer.Name, cookie);
        }

        public void Logout(string cookie)
        {
            var customer = GetCustomer(cookie);
            if (customer != null)
            {
                customer.Cookie = null;
                bankDb.SaveChanges();
                cookieManager.DeleteCookie(cookie);
            }
        }

        public bool CustomerExists(string user)
        {
            var hashedUser = HashString(user);
            return bankDb.Customers.Any(x => string.Equals(x.User, hashedUser));
        }

        Customer GetCustomer((string user, string pin) credentials)
        {
            var hashedUser = HashString(credentials.user);
            var hashedPin = HashString(credentials.pin);

            var customer = bankDb.Customers
                .FirstOrDefault(x => string.Equals(x.User, hashedUser));

            return customer != null && string.Equals(customer.Pin, hashedPin) ? customer : null;
        }

        Customer GetCustomer(string cookie)
        {
            if (cookie == null)
                throw new ArgumentNullException("Cookie cannot be null.");

            var id = cookieManager.GetUserId(cookie);
            return id > 0 ? bankDb.Customers.FirstOrDefault(x => x.Id == id) : null;
        }

        string HashString(string input)
        {
            using (SHA1 sha = SHA1.Create())
                return string.Join(string.Empty, sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        public bool IsExpired(string cookie)
        {
            if (cookie == null)
                return true;

            var id = cookieManager.GetUserId(cookie);
            if (id < 0)
                return true;

            var expTime = bankDb.Customers.FirstOrDefault(x => x.Id == id && x.Cookie == cookie)?.ExpiresOn;
            if (!expTime.HasValue)
                return true;

            return expTime.Value.AddMinutes(sessionTimeInMinutes) < DateTime.Now;
        }

        public bool IsLoggedIn(string cookie) => cookieManager.GetUserId(cookie) > 0;
    }
}