namespace Application.Interfaces;
using Application.DTOs;
public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
}