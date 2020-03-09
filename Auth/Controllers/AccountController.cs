using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IConfiguration configuration)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> CreateUser(UserInfo user)
        {
            ApplicationUser appUser = new ApplicationUser { UserName = user.Email, Email = user.Email };
            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                return BuildToken(user, new List<string>());
            } else
            {
                return BadRequest("Email or Password invalid");
            }
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserToken>> LogInUser(UserInfo user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                ApplicationUser u = await _userManager.FindByEmailAsync(user.Email);
                IList<string> roles = await _userManager.GetRolesAsync(u);
                return BuildToken(user, roles);
            }
            else
            {
                return BadRequest("Email or Password invalid");
            }
        }

        private UserToken BuildToken(UserInfo user, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("MyValue", "Mi valor que quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration };
        }
    }
}