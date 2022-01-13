using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TBashaBari.Models.ViewModel
{
    public class ContactUsVM
    {

        public ContactUsVM()
        {
            ApplicationUser = new ApplicationUser();
        }

        public ApplicationUser ApplicationUser { get; set; }

        public string Message { get; set; }
        public string Subject { get; set; }
    }
}
