using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using justAsk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace justAsk.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            ApplicationUser user = await this.userManager.FindByNameAsync(model.Username);
            if (user == default || await this.userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                return Unauthorized(new Response() { Status = "Error", Message = "Invalid username or password" });
            }
            IList<string> userRoles = await this.userManager.GetRolesAsync(user);

            List<Claim> authClaims = new List<Claim>(){
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (string role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                 issuer: this.configuration["JWT:ValidIssuer"],
                 audience: this.configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddHours(3),
                 claims: authClaims,
                 signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new
            {
                id = user.Id,
                username = user.UserName,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            ApplicationUser user = await this.userManager.FindByNameAsync(model.Username);
            if (user != default)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response() { Status = "Error", Message = "User already exists" });
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Username
            };

            var userResoult = await this.userManager.CreateAsync(newUser, model.Password);
            if (!userResoult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response() { Status = "Error", Message = "User creation failed" });
            }

            return Ok(new Response() { Status = "Success", Message = "User created successfully" });
        }
    }
}