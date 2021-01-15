using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
#nullable enable
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
