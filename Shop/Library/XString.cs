using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Shop
{
    public static class XString
    {
        public static string Str_Slug(string s)
        {
            String[][] symbols =
            {
                new String[] { "[ááàảạăắằẳằặâấầẩậ]", "a" },
                new String[] { "[đ]", "d" },
                new String[] { "[ééèẻẽẹếềểễệ]", "e" },
                new String[] { "[áàảạăắằẳằặâấầẩậ]", "a" },
                new String[] { "[óòỏõỏọôốồổỗộơớờởỡợ]", "o" },
                new String[] { "[íìỉĩị]", "i" },
                new String[] { "[úùủũụưứừửữự]", "u" },
                new String[] { "[ýỳỷỹỵ]", "y" },
                new String[] { "[\\s'\";,]", "-" },
            };
            s = s.ToLower();
            foreach (var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }
        public static string Str_Limit(this string str, int? length)
        {
            int lengt = (length ?? 20);
            if (str.Length > lengt)
            {
                str = str.Substring(0, lengt) + "...";
            }
            return str;
        }
        public static string ToMD5(this string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach(byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
    }
}
