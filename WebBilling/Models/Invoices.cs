using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebInvoicing.Models
{
    public class Invoices
    {
        public int ID { get; set; }

        [Display(Name = "Customer")]
        public string customerID { get; set; }
        public virtual UserAccount customer { get; set; }

        [Display(Name = "Invoice #")]
        public string name { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [Display(Name = "Mark as paid")]
        public bool paid { get; set; }

        public int PDFFileID { get; set; }
        public virtual PDFFile PDFFile { get; set; }
    }
}