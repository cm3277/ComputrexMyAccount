using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebInvoicing.Models
{
    public class TechAccount
    {
        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Display(Name = "Last name")]
        public string lastName { get; set; }

        [Display(Name = "Initials")]
        public string initials { get; set; }

        [Key]
        [Display(Name = "User name")]
        public string userName { get; set; }

        [Display(Name = "Hash password")]
        public string hashPassword { get; set; }

        [Display(Name = "Permission group")]
        public string permissionGroup { get; set; }
    }
}