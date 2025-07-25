﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Auth;
using ViewModels;

namespace Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<AuthResponseViewModel?> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfilePictureUrl = "default.jpg"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return null;

            await _userManager.AddToRoleAsync(user, "User");

            return await GenerateTokenAsync(user);
        }

        public async Task<AuthResponseViewModel?> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            //|| !await _userManager.CheckPasswordAsync(user, model.Password)
            if (user == null )
                return null;

            return await GenerateTokenAsync(user);
        }

        private async Task<AuthResponseViewModel> GenerateTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // Use Environment.GetEnvironmentVariable to match Program.cs
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MasjidStoryApp";
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "MasjidStoryUsers";

            // Debug: Check if JWT_KEY is null or empty
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException($"JWT_KEY environment variable is required but was {(jwtKey == null ? "null" : "empty")}. Please ensure environment variables are set in launchSettings.json or system environment.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(7);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponseViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                UserId = user.Id,
                Roles = userRoles.ToList()
            };
        }
    }
}
