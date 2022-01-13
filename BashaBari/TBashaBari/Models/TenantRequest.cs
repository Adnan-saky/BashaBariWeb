using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TBashaBari.Models
{
    public class TenantRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [DisplayName("Email")]
        public String TenantEmail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Maximum text length 100")]
        [DisplayName("Request")]
        public String RequestText { get; set; }

        [Required]
        [DisplayName("Time")]
        public String RequestTime { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Maximum text length 100")]
        [DisplayName("Comment")]
        public String CommentOnRequestText { get; set; }

        [DisplayName("Time")]
        public String CommentOnRequestTime { get; set; }

    }
}
