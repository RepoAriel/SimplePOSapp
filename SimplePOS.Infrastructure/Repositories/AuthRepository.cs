using Microsoft.AspNetCore.Identity;
using SimplePOS.Business.Exceptions;
using SimplePOS.Business.Interfaces;
using SimplePOS.Infrastructure.Authentication;
using SimplePOS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException("Usuario no encontrado");

            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                throw new Exception("Credenciales invalidas");

            return await jwtTokenGenerator.GenerateToken(user);
        }
        
        public async Task<string> RegisterAsync(string email, string password, string fullName, string role = "Empleado")
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };
            
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                    throw new AlreadyExistsException("Usuario", "Email", email);

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            await userManager.AddToRoleAsync(user, role);



            return await jwtTokenGenerator.GenerateToken(user);

        }


    }
}
