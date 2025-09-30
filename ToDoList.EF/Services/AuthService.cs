using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Core.Helpers;
using ToDoList.Core.Models;
using ToDoList.Core.Serveces;

namespace ToDoList.EF.Serveces
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManger;

        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger, IOptions<JWT> jwt)
        {
            _userManger = userManager;
            _jwt = jwt.Value;
            _roleManger = roleManger;
        }
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManger.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered" };
            }

            if (await _userManger.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "UserName Is Already registered" };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManger.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors = string.Join(",", result.Errors.Select(e => e.Description));
                }
                return new AuthModel { Message = errors };
            }

            await _userManger.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                UserName = user.UserName,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }


        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            AuthModel autModel = new AuthModel();

            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user is null || !await _userManger.CheckPasswordAsync(user, model.Password))
            {
                autModel.Message = "Email or password is incorrect";
                return autModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _userManger.GetRolesAsync(user);

            autModel.ExpireOn = jwtSecurityToken.ValidTo;
            autModel.UserName = user.UserName;
            autModel.IsAuthenticated = true;
            autModel.Email = user.Email;
            autModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            autModel.Roles = roles.ToList();
            return autModel;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManger.FindByIdAsync(model.UserId);
            if (user is null || !await _roleManger.RoleExistsAsync(model.Role))
            {
                return "UserId or Role name is invalid";
            }

            if (await _userManger.IsInRoleAsync(user, model.Role))
            {
                return "User is already assigned to this role";
            }

            var result = await _userManger.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }


        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManger.GetClaimsAsync(user);
            var roles = await _userManger.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                //roleClaims.Add(new Claim("roles", role));
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                signingCredentials: signingCredentials,
                audience: _jwt.Audience,
                claims: Claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays)
                );

            return jwtSecurityToken;
        }

    }
}
