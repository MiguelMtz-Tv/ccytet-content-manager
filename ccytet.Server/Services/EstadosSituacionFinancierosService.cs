using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.ViewModels;
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

            if(! await _context.EstadosSituacionFinanciera.AnyAsync(x => x.Periodo.Year == year && x.Periodo.Month == month && !x.Eliminado))
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

            if(await _context.EstadoSituacionFinancieraArchivos.AnyAsync(x => x.IdEstadoSituacionFinanciera == idESF && !x.Deleted)){throw new ArgumentException("No puedes eliminar un indice que aún contiene archivos.");}

            objESF.Eliminado = true;
            objESF.IdUserUpdater = id;
            objESF.UserUpdaterName = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);

            _context.EstadosSituacionFinanciera.Update(objESF);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task<List<ESFNode>> Index(dynamic data)
        {
            int year = data.year;
            string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            return await _context.EstadosSituacionFinanciera
            .Include(x => x.EstadoSituacionFinancieraArchivos)
            .Where(x => x.Periodo.Year == year && !x.Eliminado)
            .OrderBy(x => x.Periodo.Month)
            .Select(x => new ESFNode {
                Level = 0,
                Id = x.IdEstadoSituacionFinanciera,
                Title = meses[x.Periodo.Month - 1],
                Key = x.Periodo.ToString("G"),
                Children = x.EstadoSituacionFinancieraArchivos.Where(x => !x.Deleted).Select(x => new ESFNode
                {
                    Level = 1,
                    Id = x.IdEstadoSituacionFinancieraArchivo,
                    Title = x.Titulo,
                    Key = x.Path,
                    Children = null
                }).ToList()
            }).ToListAsync();
        }

        public async Task AddFiles(ClaimsPrincipal user, ReqAddFiles data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();
            string id = Globals.GetClaim("Id", user);

            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            EstadoSituacionFinanciera objESF = await _context.EstadosSituacionFinanciera.AsNoTracking().FirstOrDefaultAsync(x => x.IdEstadoSituacionFinanciera == data.idEstadoSituacionFinanciera);

            List<EstadoSituacionFinancieraArchivo> lstAddedFiles =new();
            try
            {
                foreach(ReqAddFiles.File file in data.files)
                {
                    string fileName = $"{objESF.Periodo.Month}_{objESF.Periodo.Year}_{file.name}";

                    byte[] bytes = Convert.FromBase64String(file.base64.Split(",")[1]);
                    string storagePath = Path.Combine("Public", "ESF", fileName);


                    //guarda el archivo si no existe, si no, sobreescribe el archivo que se encuentra en el directorio
                    if(!File.Exists(storagePath))
                    {
                        using(FileStream stream = new FileStream(storagePath, FileMode.Create))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Flush();
                        }

                        lstAddedFiles.Add(new(){
                            IdEstadoSituacionFinancieraArchivo  = Guid.NewGuid().ToString(),
                            Titulo                              = fileName,
                            Path                                = storagePath,
                            Deleted                             = false,
                            FechaCreacion                       = DateTime.Now,
                            IdEstadoSituacionFinanciera         = data.idEstadoSituacionFinanciera,
                            IdUserCreator                       = objUser.Id,
                        });
                    }
                    else
                    {
                        File.Delete(storagePath);
                        using(FileStream stream = new FileStream(storagePath, FileMode.Create))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Flush();
                        }
                    }
                }

                await _context.EstadoSituacionFinancieraArchivos.AddRangeAsync(lstAddedFiles);
                await _context.SaveChangesAsync();
                await loginTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                lstAddedFiles.ForEach(x => {
                    string path = Path.Combine(x.Path);
                    if(File.Exists(path)){
                        File.Delete(path);
                    }
                });

                throw new ArgumentException("Error al crear archivos: " + ex.Message);
            }
        }

        public async Task DeleteFile(ClaimsPrincipal user, dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = Globals.GetClaim("Id", user);
            string idFile = data.id;

            EstadoSituacionFinancieraArchivo objFile = await _context.EstadoSituacionFinancieraArchivos.FirstOrDefaultAsync(x => x.IdEstadoSituacionFinancieraArchivo == idFile);
            objFile.Deleted = true;
            objFile.FechaEliminación = DateTime.Now;
            objFile.IdUserDeleter = id;

            string deletePath = Path.Combine(objFile.Path);

            if(!File.Exists(deletePath)){throw new ArgumentException("No se encontró el archivo a eliminar. Si este archivo continúa siendo visible, contacte al administrador");}
            File.Delete(deletePath);

            _context.EstadoSituacionFinancieraArchivos.Update(objFile);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }
    }
}