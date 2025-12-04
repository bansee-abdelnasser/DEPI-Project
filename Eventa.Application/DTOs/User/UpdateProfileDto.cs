using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.DTOs.User
{
    public class UpdateProfileDto
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
