using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(string email, string password, string fullName);
        Task<string> LoginAsync(string email, string password);
    }
}
