using System.Security.Claims;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class BuggyController : BaseAPIController
{
    [AllowAnonymous]
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorize()
    {
        return Unauthorized();
    }
    [AllowAnonymous]
    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("TEST BAD REQUESTTTT");
    }
    [AllowAnonymous]
    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }
    [AllowAnonymous]
    [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
        throw new Exception("TEST INTERNAL ERRORRRRRRR");
    }
    [AllowAnonymous]
    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDTO product)
    {
        return Ok();
    }

    [HttpGet("testauth")]
    public IActionResult GetSecretText()
    {
        var userName = User.Identity?.Name;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok($"secret text for {userName} with id {id}");
    }
}
