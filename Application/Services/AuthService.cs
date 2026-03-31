using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class AuthService : IAuthService
{
    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        // TEMP logic (we will improve later)
        return "User registered successfully";
    }
}