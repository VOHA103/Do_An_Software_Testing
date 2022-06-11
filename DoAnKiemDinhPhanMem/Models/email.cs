using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnKiemDinhPhanMem.Models
{
    public class email
    {
        public string mail { get; set; }
        public string MK { get; set; }
        public email() { }
        public email(string m, string mk)
        {
            this.mail = m;
            this.MK = mk;
        }
    }
}