using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Advanced.Controllers;

[Route("api/account")]
[ApiController]
public class ApiAccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public ApiAccountController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
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
