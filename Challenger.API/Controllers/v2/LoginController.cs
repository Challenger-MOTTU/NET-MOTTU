using Asp.Versioning;
using Challenger.Application.DTOs.Responses;
using Challenger.Application.UseCase.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LoginRequest = Challenger.Application.DTOs.Requests.LoginRequest;

[Route("api/v{version:apiVersion}/login")]
[ApiController]
[ApiVersion("2.0")]
[Produces("application/json")]
[SwaggerTag("Gerenciamento de Login")]
[AllowAnonymous]
public class LoginController(ILoginUseCase loginUseCase) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Login Service", Description = "Get Token")]
    [SwaggerResponse(StatusCodes.Status200OK, "Token returned")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error")]

    public async Task<LoginResponse> Login([FromBody] LoginRequest loginDto)
    {
        return await loginUseCase.Login(loginDto);
    }
}
