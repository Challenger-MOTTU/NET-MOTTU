using Challenger.Application.DTOs.Requests;
using Challenger.Application.DTOs.Responses;


namespace Challenger.Application.UseCase.Login;

public interface ILoginUseCase
{
    Task<LoginResponse?> Login(LoginRequest request);
}