using System;
using System.Collections.Generic;

namespace SimpleBanking
{
    public interface ICookieManager
    {
        void SaveCookie(string cookie, int id);
        void DeleteCookie(string cookie);
        int GetUserId(string cookie);
    }

    public class CookieManager : ICookieManager
    {
        readonly Dictionary<string, int> cookieCollection = new Dictionary<string, int>();

        public void DeleteCookie(string cookie)
        {
            if (cookie == null)
                throw new ArgumentNullException("Cookie cannot be null.");

            if (cookieCollection.ContainsKey(cookie))
                cookieCollection.Remove(cookie);
        }

        public int GetUserId(string cookie)
        {
            if (cookie != null && cookieCollection.ContainsKey(cookie))
                return cookieCollection[cookie];
            else
                return -1;
        }

        public void SaveCookie(string cookie, int id)
        {
            if (cookie == null)
                throw new ArgumentNullException("Cookie cannot be null.");

            if (!cookieCollection.ContainsKey(cookie))
                cookieCollection.Add(cookie, id);
        }
    }
}