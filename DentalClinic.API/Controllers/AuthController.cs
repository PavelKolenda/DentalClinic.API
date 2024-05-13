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
    public async Task<IActionResult> Register([FromBody] PatientCreateDto patientCreateDto)
    {
        string token = await _authService.Register(patientCreateDto);

        return Ok(token);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] PatientLoginDto patientLoginDto)
    {
        var token = await _authService.Login(patientLoginDto);

        return Ok(token);
    }
}
