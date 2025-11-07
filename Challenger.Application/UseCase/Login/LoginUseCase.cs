using Challenger.Application.DTOs.Requests;
using Challenger.Application.DTOs.Responses;
using Challenger.Application.Services;
using Challenger.Domain.Interfaces;


namespace Challenger.Application.UseCase.Login;

public class LoginUseCase(IUserRepository userRepository, ITokenService tokenService): ILoginUseCase
{
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        
        if (user is null )
            return await Task.FromResult(new  LoginResponse("Invalid credentials or blocked account"));
        
    
        return await Task.FromResult(new LoginResponse
        {
            Message  = "Login successful",
            Token = tokenService.GenerateToken(user),
            Id = user.Id
        });
    }
}