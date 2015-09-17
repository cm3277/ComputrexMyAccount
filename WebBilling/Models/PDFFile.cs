using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebInvoicing.Models;

namespace WebInvoicing.Models
{
    public class PDFFile
    {
        public int ID { get; set; }
        public byte[] Content { get; set; }
    }
}