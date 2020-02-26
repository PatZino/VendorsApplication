using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDbSpCallMVC.Models.DB
{
    public class SpVendors
    {
        [Required]
        public int VendorId { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Vendor Name can not be blank")]
        [Required]
        [Display(Name = "Vendor\'s Name")]
        public string Name { get; set; }
    }
}
