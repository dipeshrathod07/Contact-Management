using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contact_management.Models
{
    public class Contact
    {
        public int c_contactid { get; set; }
        public int c_userid { get; set; }
        public string c_contactname { get; set; }
        public string c_email { get; set; }
        public string c_address { get; set; }
        public string? c_image { get; set; }
        public string c_status { get; set; }
        public string c_mobile { get; set; }
        public string c_group { get; set; }
        
        public IFormFile ProfileImage{  get; set; }
    }
}