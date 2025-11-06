namespace Challenger.Application.DTOs.Responses;

public class LoginResponse
{
    public Guid Id { get; set; }
    
    public string Token { get; set; }
    
    public string Message { get; set; }
    
    public LoginResponse(string message)
    {
        Message = message;
    } 
    

    public LoginResponse()
    {
        
    }
}