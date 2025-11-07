using System.ComponentModel.DataAnnotations;
using Challenger.Domain.Entities;

namespace Challenger.Application.DTOs.Requests;

public class UserRequest
{
    [Required(ErrorMessage = "É necessário um nome de Usuário")]
    public string Username { get; set; }
    [Required(ErrorMessage = "E-mail é obrigatório")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
}