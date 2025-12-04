using Eventa.Application.DTOs.User;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Eventa.DataAccess.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.Services
{
    internal class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<IdentityResult> Register(UserRegisterDto user)
        {
            AppUser identityUser = new AppUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,

                Email = user.Email,
                UserName = user.UserName,
                IsExternalUser = false
            };

            var result = await _uow.UserManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                if (user.Roles != null && user.Roles.Any())
                {
                    foreach (var role in user.Roles)
                    {
                        result = await _uow.UserManager.AddToRoleAsync(identityUser, role);
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Error add user {user.UserName} to role {role}.");
                        }
                    }
                }
                else
                {
                    result = await _uow.UserManager.AddToRoleAsync(identityUser, "users");
                }
            }
            return result;


        }

        public async Task<UserLoginResult> LoginAsync(UserLoginDto user)
        {


            var identityUser = await _uow.UserManager.FindByNameAsync(user.Username);
            var loginResult = new UserLoginResult();
            if (identityUser == null)
            {
                loginResult.Errors.Add("Invalid User Name!.");
                return loginResult;
            }
            var result = await _uow.UserManager.CheckPasswordAsync(identityUser, user.Password);
            if (result == false)
            {
                loginResult.Errors.Add("Invalid Password!.");
                return loginResult;
            }

            var token = await _uow.TokenManager.GetTokenAsync(identityUser);

            var refreshToken = _uow.TokenManager.GetRefreshToken();

            identityUser.RefreshToken = refreshToken.Token;
            identityUser.RefreshTokenExpiryTime = refreshToken.TokenExpiryTime;

            await _uow.UserManager.UpdateAsync(identityUser);

            loginResult.UserName = identityUser.UserName;
            loginResult.Claims = token.Claims;

            loginResult.TokenResult.Token = token.Token;
            loginResult.TokenResult.TokenExpiryTime = token.TokenExpiryTime;

            loginResult.TokenResult.RefreshToken = refreshToken.Token;
            loginResult.TokenResult.RefreshTokenExpiryTime = refreshToken.TokenExpiryTime;



            loginResult.Succeeded = true;
            return loginResult;
        }

        public async Task<AppUser?> GetUserByIdAsync(string userId)
        {
            return await _uow.UserManager.FindByIdAsync(userId);
        }

        public async Task<ProfileUpdateResultDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var user = await _uow.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ProfileUpdateResultDto
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            bool IsInvalidSwaggerValue(string v) =>
                string.IsNullOrWhiteSpace(v) || v == "string";


            if (!IsInvalidSwaggerValue(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!IsInvalidSwaggerValue(dto.LastName))
                user.LastName = dto.LastName;



            if (!IsInvalidSwaggerValue(dto.Username) && dto.Username != user.UserName)
            {
                var existingUser = await _uow.UserManager.FindByNameAsync(dto.Username);
                if (existingUser != null && existingUser.Id != userId)
                {
                    return new ProfileUpdateResultDto
                    {
                        Success = false,
                        Message = "Username already taken."
                    };
                }

                user.UserName = dto.Username;
                user.NormalizedUserName = dto.Username.ToUpper();
            }



            if (user.IsExternalUser)
            {
                //external users (Google/Facebook) cannot update email
                if (!IsInvalidSwaggerValue(dto.Email) && dto.Email != user.Email)
                {
                    return new ProfileUpdateResultDto
                    {
                        Success = false,
                        Message = "External login users cannot change email."
                    };
                }
            }
            else
            {
                //local users can update email
                if (!IsInvalidSwaggerValue(dto.Email) && dto.Email != user.Email)
                {
                    if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(dto.Email))
                    {
                        return new ProfileUpdateResultDto
                        {
                            Success = false,
                            Message = "Invalid email format."
                        };
                    }

                    var existingEmail = await _uow.UserManager.FindByEmailAsync(dto.Email);
                    if (existingEmail != null && existingEmail.Id != userId)
                    {
                        return new ProfileUpdateResultDto
                        {
                            Success = false,
                            Message = "Email already in use."
                        };
                    }

                    user.Email = dto.Email;
                    user.NormalizedEmail = dto.Email.ToUpper();
                }
            }



            if (dto.ProfilePicture != null)
            {
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    var oldPath = Path.Combine("wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ProfilePicture.FileName)}";
                var folder = Path.Combine("wwwroot", "images", "profile");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/images/profile/{fileName}";
            }



            if (!IsInvalidSwaggerValue(dto.NewPassword))
            {
                bool hasPassword = await _uow.UserManager.HasPasswordAsync(user);

                if (!hasPassword)
                {
                    // Social login user (Google/Facebook) adding a password for the first time
                    var addPass = await _uow.UserManager.AddPasswordAsync(user, dto.NewPassword);

                    if (!addPass.Succeeded)
                    {
                        return new ProfileUpdateResultDto
                        {
                            Success = false,
                            Message = "Failed to set password.",
                            Errors = addPass.Errors.Select(e => e.Description).ToList()
                        };
                    }
                }
                else
                {
                    // Normal users changing password
                    if (IsInvalidSwaggerValue(dto.OldPassword))
                    {
                        return new ProfileUpdateResultDto
                        {
                            Success = false,
                            Message = "Old password is required."
                        };
                    }

                    var changePass = await _uow.UserManager.ChangePasswordAsync(
                        user,
                        dto.OldPassword,
                        dto.NewPassword
                    );

                    if (!changePass.Succeeded)
                    {
                        return new ProfileUpdateResultDto
                        {
                            Success = false,
                            Message = "Password update failed.",
                            Errors = changePass.Errors.Select(e => e.Description).ToList()
                        };
                    }
                }
            }


            var updateResult = await _uow.UserManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new ProfileUpdateResultDto
                {
                    Success = false,
                    Message = "Failed to update profile.",
                    Errors = updateResult.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ProfileUpdateResultDto
            {
                Success = true,
                Message = "Profile updated successfully.",
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImageUrl = user.ProfilePictureUrl
            };
        }

        public async Task<bool> DeleteProfilePictureAsync(string userId)
        {
            var user = await _uow.UserManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            if (string.IsNullOrEmpty(user.ProfilePictureUrl))
                return false;

            // Build full file path
            var oldPath = Path.Combine("wwwroot", user.ProfilePictureUrl.TrimStart('/'));

            // Delete from wwwroot if exists
            if (File.Exists(oldPath))
                File.Delete(oldPath);

            // Remove from database
            user.ProfilePictureUrl = null;

            await _uow.UserManager.UpdateAsync(user);
            

            return true;
        }

        public async Task<IdentityResult> SwitchToOrganizerAsync(string userId)
        {
            var user = await _uow.UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Optionally remove from "users" role
            if (await _uow.UserManager.IsInRoleAsync(user, "users"))
                await _uow.UserManager.RemoveFromRoleAsync(user, "users");

            // Add to "organizers" role
            var result = await _uow.UserManager.AddToRoleAsync(user, "organizers");

            return result;
        }

        public async Task<IdentityResult> SwitchToUserAsync(string userId)
        {
            var user = await _uow.UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Remove from "organizers" role if exists
            if (await _uow.UserManager.IsInRoleAsync(user, "organizers"))
                await _uow.UserManager.RemoveFromRoleAsync(user, "organizers");

            // Add to "users" role if not exists
            if (!await _uow.UserManager.IsInRoleAsync(user, "users"))
                return await _uow.UserManager.AddToRoleAsync(user, "users");

            return IdentityResult.Success;
        }

    }
}
