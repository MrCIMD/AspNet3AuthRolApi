using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Context;
using Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost("SetRoleUser")]
        public async Task<ActionResult> SetRoleUser(RoleDTO roleDTO)
        {
            var user = await userManager.FindByIdAsync(roleDTO.UserId);
            if (user == null) { return NotFound(); }
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleDTO.RoleName));
            await userManager.AddToRoleAsync(user, roleDTO.RoleName);
            return Ok();
        }

        [HttpPost("RemoveRoleUser")]
        public async Task<ActionResult> RemoveRoleUser(RoleDTO roleDTO)
        {
            var user = await userManager.FindByIdAsync(roleDTO.UserId);
            if (user == null) { return NotFound(); }
            await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, roleDTO.RoleName));
            await userManager.RemoveFromRoleAsync(user, roleDTO.RoleName);
            return Ok();
        }
    }
}