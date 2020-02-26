using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreDbSpCallMVC.Models.DB
{
    public partial class TblVendor
    {
        [Required]
        public int VendorId { get; set; }
        [Required]
        [Display(Name = "Vendor\'s Name")]
        public string Name { get; set; }
    }
}
