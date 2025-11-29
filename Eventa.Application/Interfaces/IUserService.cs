using Eventa.Application.DTOs.User;
using Eventa.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> Register(UserRegisterDto user);

        Task<UserLoginResult> LoginAsync(UserLoginDto user);

        Task<AppUser?> GetUserByIdAsync(string userId);
        Task<IdentityResult> SwitchToOrganizerAsync(string userId);
        Task<IdentityResult> SwitchToUserAsync(string userId);
        Task<ProfileUpdateResultDto> UpdateProfileAsync(string userId,UpdateProfileDto dto);

        Task<bool> DeleteProfilePictureAsync(string userId);
    }
}
