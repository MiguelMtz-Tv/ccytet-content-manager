using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.ViewModels.Req.ESF;
using Microsoft.EntityFrameworkCore;
using Workcube.Libraries;

namespace ccytet.Server.Services
{
    public class EstadosSituacionFinancierosService
    {
        private readonly DataContext _context;
        public EstadosSituacionFinancierosService(DataContext context)
        {
            _context = context;
        }

        public async Task Store(ClaimsPrincipal user ,ReqCreateESF data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();
            string id = Globals.GetClaim("Id", user);

            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            int year = data.periodo.Year;
            int month = data.periodo.Month;

            if(! await _context.EstadosSituacionFinanciera.AnyAsync(x => x.Periodo.Year == year && x.Periodo.Month == month))
            {
                EstadoSituacionFinanciera objESF = new()
                {
                    IdEstadoSituacionFinanciera = Guid.NewGuid().ToString(),
                    Periodo                     = data.periodo,
                    Eliminado                   = false,
                    IdUserCreator               = objUser.Id,
                    IdUserUpdater               = objUser.Id,
                    UserCreatorName             = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos),
                    UserUpdaterName             = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos),
                };

                await _context.EstadosSituacionFinanciera.AddAsync(objESF);
                await _context.SaveChangesAsync();
                await loginTransaction.CommitAsync();
            }
            else
            {
                throw new ArgumentException("Ya existe un estado de situación financiera de este periodo");
            }
        }

        public async Task Delete(ClaimsPrincipal user, dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string idESF = data.id;
            string id = Globals.GetClaim("Id", user);

            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            EstadoSituacionFinanciera objESF = await _context.EstadosSituacionFinanciera.FindAsync(idESF);

            objESF.Eliminado = false;
            objESF.IdUserUpdater = id;
            objESF.UserUpdaterName = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);

            _context.EstadosSituacionFinanciera.Update(objESF);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }
    }
}