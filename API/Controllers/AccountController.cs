using System;
using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager) : BaseAPIController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO registerDTO)
    {
        AppUser user = new()
        {
            UserName = registerDTO.Email,
            Email = registerDTO.Email,
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName
        };

        IdentityResult result = await signInManager.UserManager.CreateAsync(user, registerDTO.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return BadRequest(new { errors });
        }
        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        //does include remove auth cookie in browser as well 
        await signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult<AppUser>> GetUserInfo()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return NoContent();
        }

        AppUser? user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Address?.ToAddressDTO()
        });
    }

    [HttpGet("auth-status")]
    public ActionResult GetAuthState()
    {
        return Ok(new { IsAuthenticated = User.Identity != null && User.Identity.IsAuthenticated });
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<AddressDTO>> CreateOrUpdateUserAddress(AddressDTO addressDTO)
    {
        AppUser? user = await signInManager.UserManager.GetUserByEmailWithAddress(User);
        if (user == null) return Unauthorized();
        if (user.Address == null)
        {
            user.Address = addressDTO.ToEntity();
        }
        else
        {
            user.Address.UpdateFromDTO(addressDTO);
        }

        IdentityResult result = await signInManager.UserManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(user.Address?.ToAddressDTO());
        }

        return BadRequest("Problem updating user address");
    }
}
