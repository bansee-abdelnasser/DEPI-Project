using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.DTOs.User
{
    public class ProfileUpdateResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }


        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ProfileImageUrl { get; set; }

        public List<string>? Errors { get; set; }
    }
}
