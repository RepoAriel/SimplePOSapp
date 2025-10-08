using SimplePOS.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(UserRegisterRequest request);
        Task<AuthResponse> LoginAsync(UserLoginRequest request);
    }
}
