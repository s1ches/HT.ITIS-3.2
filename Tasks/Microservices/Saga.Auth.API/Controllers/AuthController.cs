using Microsoft.AspNetCore.Mvc;
using Saga.Auth.API.Models.Auth.Register;

namespace Saga.Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<long> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        
    }
}