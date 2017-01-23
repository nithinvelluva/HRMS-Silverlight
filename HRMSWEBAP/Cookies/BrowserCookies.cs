using System;
using System.Windows.Browser;

namespace HRMSWEBAP.Cookies
{
    public class BrowserCookies
    {
        /// <summary>
        /// sets a persistent cookie with an expiration time of 1 hour
        /// </summary>
        /// <param name="key">the cookie key</param>
        /// <param name="value">the cookie value</param>
        public static void SetCookie(string key, string usrid, string usrtype)
        {
            string cookieName = key;
            string oldCookie = HtmlPage.Document.GetProperty(cookieName) as string;
            DateTime expiration = DateTime.Now + TimeSpan.FromHours(1);
            string cookie = string.Format("{0}={1};{0}={2};expires={3}", key, usrid, usrtype, expiration.ToString("R"));
            HtmlPage.Document.SetProperty(cookieName, cookie);
        }

        // <summary>
        /// Deletes a specified cookie by setting its value to empty and expiration to -1 hours
        /// </summary>
        /// <param name="key">the cookie key to delete</param>
        public static void DeleteCookie(string key)
        {
            string cookieName = key;
            string oldCookie = HtmlPage.Document.GetProperty(cookieName) as string;
            DateTime expiration = DateTime.UtcNow - TimeSpan.FromHours(1);
            string cookie = string.Format("{0}=;{0}=;expires={1}", key, expiration.ToString("R"));
            HtmlPage.Document.SetProperty(cookieName, cookie);
        }

        /// <summary>
        /// Retrieves an existing cookie
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <returns>null if the cookie does not exist, otherwise the cookie value</returns>
        public static string GetCookie(string key)
        {
            string cks = HtmlPage.Document.GetProperty(key) as string;
            if (cks != null && cks != "")
            {
                string[] cookies = cks.Split(';');
                string[] cookies1 = HtmlPage.Document.Cookies.Split(';');
                key += '=';
                if (cookies.Length == 3)
                {
                    string[] cookieexpire = cookies[2].Split(',');
                    DateTime expiredate = Convert.ToDateTime(cookieexpire[1]);
                    DateTime dt = DateTime.Now;
                    if (dt < expiredate)
                    {
                        string rslt = string.Empty;
                        foreach (string cookie in cookies)
                        {
                            string cookieStr = cookie.Trim();
                            if (cookieStr.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                            {
                                string[] vals = cookieStr.Split('=');

                                if (vals.Length >= 2)
                                {
                                    rslt += vals[1] + ";";
                                }
                                else
                                {
                                    return string.Empty;
                                }
                            }
                        }
                        return rslt;
                    }
                }
            }
            return null;
        }
    }
}
