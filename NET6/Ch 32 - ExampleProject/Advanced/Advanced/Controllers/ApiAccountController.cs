using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Advanced.Controllers;

[Route("api/account")]
[ApiController]
public class ApiAccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public ApiAccountController(SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]Credentials creds)
    {
        Microsoft.AspNetCore.Identity.SignInResult result
            = await _signInManager.PasswordSignInAsync(creds.Username, creds.Password, false, false);

        if(result.Succeeded)
        {
            return Ok();
        }
        return Unauthorized();
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody]Credentials creds)
    {
        if(await CheckPassword(creds))
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] secret = Encoding.ASCII.GetBytes(_configuration["jwtSecret"]);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, creds.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = handler.CreateToken(descriptor);
            return Ok(new
            {
                success = true,
                token = handler.WriteToken(token)
            });
        }
        return Unauthorized();
    }

    private async Task<bool> CheckPassword(Credentials creds)
    {
        IdentityUser user = await _userManager.FindByNameAsync(creds.Username);
        if(user !=null)
        {
            return (await _signInManager.CheckPasswordSignInAsync(user, creds.Password, true)).Succeeded;
        }
        return false;
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    public class Credentials
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
