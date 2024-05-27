using DentalClinic.Services.Auth;
using DentalClinic.Shared.DTOs.Patients;

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
}
