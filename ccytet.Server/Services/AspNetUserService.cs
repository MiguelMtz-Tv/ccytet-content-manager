using ccytet.Server.Data;
using ccytet.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ccytet.Server.Services
{
    public class AspNetUserService
    {
        private readonly DataContext _context;
        public AspNetUserService(DataContext context)
        {
            _context = context;
        }

        public async Task<dynamic> Create(dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();
            string password = data.password;

            //Encriptamos la contraseña
            var passwordHasher = new PasswordHasher<AspNetUser>();
            string hashedPassword = passwordHasher.HashPassword(new AspNetUser(), password);

            AspNetUser newAspNetUser = new (){
                Nombre                  = data.nombres,
                Apellidos               = data.apellidos,
                PasswordHash            = hashedPassword,
                Matricula               = data.matricula,

                EmailConfirmed          = true,
                PhoneNumberConfirmed    = true,
                TwoFactorEnabled        = true,
                LockoutEnabled          = false,
                AccessFailedCount       = 0,
            };

            try
            {
                await _context.AspNetUsers.AddAsync(newAspNetUser);
                await _context.SaveChangesAsync();
                await loginTransaction.CommitAsync();

                return newAspNetUser;
            }
            catch(Exception ex)
            {
                throw new ArgumentException("No fue posible añadir al usuario " + ex.Message );
            }
        }

        public async Task<AspNetUser> FindLogin(string matricula)
        {
            return await _context.AspNetUsers.Where(u => u.Matricula == matricula).FirstOrDefaultAsync();
        }
    }
}