using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebInvoicing.Models
{
    public class UserAccount
    {
        [Key]
        [Display(Name = "User name")]
        public string userName { get; set; }

        [Display(Name = "Password")]
        public string hashPassword { get; set; }

        [Display(Name = "Permission group")]
        public string permissionGroup { get; set; }

        public virtual ICollection<Invoices> Invoices { get; set; }
    }
}