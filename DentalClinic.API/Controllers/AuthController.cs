using DentalClinic.Services.Auth;
using DentalClinic.Shared.DTOs.Patients;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] PatientCreateDto patientCreateDto)
    {
        var response = await _authService.Register(patientCreateDto);

        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] PatientLoginDto patientLoginDto)
    {
        var response = await _authService.Login(patientLoginDto);

        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<AuthResponse>> GetCurrentUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId = int.Parse(userIdClaim.Value);

        AuthResponse authResponse = await _authService.GetCurrentUser(userId);
        return authResponse;
    }
}
