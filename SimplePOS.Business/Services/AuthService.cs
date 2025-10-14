using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepo;

        public AuthService(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;
        }

        public async Task<AuthResponse> LoginAsync(UserLoginRequest request)
        {
            var token = await authRepo.LoginAsync(request.Email, request.Password);
            return new AuthResponse
            {
                Email = request.Email,
                Token = token
            };
        }

        public async Task<AuthResponse> RegisterAsync(UserRegisterRequest request, string role = "Empleado")
        {
            var token = await authRepo.RegisterAsync(request.Email, request.Password, request.FullName, role);
            return new AuthResponse
            {
                Email = request.Email,
                Token = token
            };

            
        }
    }
}
