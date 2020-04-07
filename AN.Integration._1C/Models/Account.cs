using System;
using System.Collections.Generic;
using System.Text;

namespace AN.Integration._1C.Models
{
    public class Account
    {
        [StringLength(4, ErrorMessage = "Product code max length is exceeded")]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
