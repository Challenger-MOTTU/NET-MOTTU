using Challenger.Application.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace Challenger.Application.UseCase.Login;

public interface ILoginUseCase
{
    Task<LoginResponse?> Login(LoginRequest request);
}