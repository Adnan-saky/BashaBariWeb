using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TBashaBari.Models
{
    public class TenantConnectsOwner
    {
        [Key]
        public int ConnectionId { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Tenant Email")]
        public String TenantEmail { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Owner Email")]
        public String OwnerEmail { get; set; }

        [Required]
        [DisplayName("Confirmation")]
        public String IsConfirmed { get; set; }
    }
}
