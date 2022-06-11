using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace DoAnKiemDinhPhanMem.Models
{
    public class User
    {
        public static string convertVND(string money)
        {
            var format = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            string value = String.Format(format, "{0:c0}", Convert.ToUInt32(money));
            return value;
        }
        public static string EncodeMD5(string pass)
        {
            try
            {

                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
                bs = md5.ComputeHash(bs);
                System.Text.StringBuilder s = new System.Text.StringBuilder();

                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x1").ToLower());
                }
                pass = s.ToString();
                return pass;
            }
            catch (Exception ex)
            {

                return "";
            }
        }
        public static string EncodeSHA1(string pass)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
            bs = sha1.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x1").ToLower());
            }

            pass = s.ToString();
            return pass;
        }
    }
}