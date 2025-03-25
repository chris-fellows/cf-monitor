using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    /// <summary>
    /// Password reset details
    /// </summary>
    public class PasswordReset
    {
        [Required]
        [MaxLength(50)]
        public string Id { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        public string UserId { get; set; } = String.Empty;

        /// <summary>
        /// Validation Id which is appended to the URL to make it harder to manually craft a working URL
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string ValidationId { get; set; } = String.Empty;

        /// <summary>
        /// URL to reset password
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Url { get; set; } = String.Empty;

        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// Expiry time for this reset
        /// </summary>
        public DateTimeOffset ExpiresDateTime { get; set; }
    }
}
