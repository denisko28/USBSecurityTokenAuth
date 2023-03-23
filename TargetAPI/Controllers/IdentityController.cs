using System.Security;
using Microsoft.AspNetCore.Mvc;
using TargetAPI.DTOs;
using TargetAPI.Exceptions;
using TargetAPI.Repositories.Abstract;

namespace TargetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityRepository _identityRepository;

    public IdentityController(IIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
    }

    [HttpPost("signIn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> SignIn([FromBody] SignInRequest request)
    {
        try
        {
            var user = await _identityRepository.GetUserByLogin(request.Login);
            if (user.Hash != request.Hash)
                throw new SecurityException("Access denied!");
            
            return Ok("Вітаємо, " + user.Surname + " " + user.Name + "!");
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { e.Message });
        }
        catch (SecurityException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
        }
    }
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var successful = await _identityRepository.AddUser(request);
            if (!successful)
                throw new Exception("Помилка! Користувач з таким логіном вже зареєстрований!");
                
            return Ok("Користувача успішно зареєстровано!");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
        }
    }
}