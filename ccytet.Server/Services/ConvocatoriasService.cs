using System.Runtime.CompilerServices;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.ViewModels;
using ccytet.Server.ViewModels.Req.Convocatorias;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Workcube.Libraries;

namespace ccytet.Server.Services
{
    public class ConvocatoriasService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ConvocatoriasService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Store(ClaimsPrincipal user, CrearConvocatoriaReq.Root data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = Globals.GetClaim("Id", user);
            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            string userFullName = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);

            Convocatoria convocatoria = new();
            convocatoria.IdConvocatoria     = Guid.NewGuid().ToString();
            convocatoria.Autor              = userFullName;
            convocatoria.Titulo             = data.titulo;
            convocatoria.Texto              = data.body;
            convocatoria.IdUserCreator      = id;
            convocatoria.UserCreatorName    = userFullName;
            convocatoria.IdUserUpdater      = id;
            convocatoria.UserUpdaterName    = userFullName;
            convocatoria.FechaCreacion      = DateTime.Now;
            convocatoria.FechaActualizacion = DateTime.Now;
            convocatoria.Abierto            = true;

            /* 
            *---------------------------
            | Almacenamiento de portada
            *---------------------------
            */
            string portadaPath  = Path.Combine("Public", "Convocatorias", "Portadas");
            string fileName     = $"{Guid.NewGuid().ToString()}.png";
            string storagePath  = Path.Combine(portadaPath, fileName);

            byte[] portadaBytes = Convert.FromBase64String(data.portada.Split(",")[1]);

            using(FileStream stream = new(storagePath, FileMode.Create))
            {
                stream.Write(portadaBytes, 0, portadaBytes.Length);
                stream.Flush();
            }

            convocatoria.PortadaPath = storagePath;
            
            /* 
            *---------------------------
            | Almacenamiento de archivos
            *---------------------------
            */

            List<string> files = new List<string>();
            foreach(CrearConvocatoriaReq.File file in data.files)
            {
                string path = Path.Combine("Public", "Convocatorias", "Recursos");
                if(!string.IsNullOrEmpty(file.base64))
                {
                    string fileN = $"{Guid.NewGuid().ToString().Split("-")[3]}_{file.name}";
                    string storage = Path.Combine(path, fileN);
                    byte[] bytes = Convert.FromBase64String(file.base64.Split(',')[1]);

                    using(FileStream stream = new(storage, FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                    files.Add(storage);
                }
            }

            convocatoria.FilesArray = JsonConvert.SerializeObject(files);

            await _context.Convocatorias.AddAsync(convocatoria);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task<dynamic> DataSource(dynamic data)
        {
            IQueryable<ConvocatoriaViewModel> lstData = DataSourceExpression(data);
            DataSourceBuilder<ConvocatoriaViewModel> objDataSourcebuilder = new(data, lstData);

            var objDataBuilder = await objDataSourcebuilder.build();

            // CONSTRUCCIÓN DE RETORNO DE DATOS
            List<ConvocatoriaViewModel> lstOriginal = objDataBuilder.rows;
            List<dynamic> rows = new();

            lstOriginal.ForEach(item =>
                rows.Add(new {
                    item.IdConvocatoria,
                    item.Titulo,
                    FechaCreacion = item.FechaCreacionNatural,
                    FechaActualizacion = item.FechaActualizacionNatural,
                    item.UserCreatorName,
                    item.UserUpdaterName,
                    item.PortadaPath,
                    item.Abierto,
                    item.Eliminado
                })
            );

            var objResult = new
            {
                rows    = rows,
                count   = objDataBuilder.count,
                length  = objDataBuilder.length,
                pages   = objDataBuilder.pages,
                page    = objDataBuilder.page,
            };

            return objResult;
        }

        public IQueryable<ConvocatoriaViewModel> DataSourceExpression(dynamic data)
        {
            IQueryable<ConvocatoriaViewModel> query;
            IQueryable<Convocatoria> rows = _context.Convocatorias.AsNoTracking().OrderByDescending(x => x.FechaCreacion);

            if(!String.IsNullOrEmpty(data.dateFrom.Value))
            {
                string arg = data.dateFrom.Value;
                DateTime dateFrom =  DateTime.Parse(arg);
                rows = rows.Where(x => x.FechaCreacion >= dateFrom);
            }

            if(!String.IsNullOrEmpty(data.dateTo.Value))
            {
                string arg = data.dateTo.Value;
                DateTime dateTo =  DateTime.Parse(arg);
                rows = rows.Where(x => x.FechaCreacion <= dateTo);
            }

            if(!String.IsNullOrEmpty(data.search.Value))
            {
                string arg = data.search.Value;
                rows = rows.Where(x => x.Titulo.Contains(arg) || x.Texto.Contains(arg) || x.UserCreatorName.Contains(arg) || x.UserUpdaterName.Contains(arg));
            }

            query = rows.ProjectTo<ConvocatoriaViewModel>(_mapper.ConfigurationProvider);
            return query;
        }

        public async Task ToggleStatus(ClaimsPrincipal user, dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = Globals.GetClaim("Id", user);
            string idConvocatoria = data.idConvocatoria;

            AspNetUser objUser = await _context.AspNetUsers.FirstOrDefaultAsync(x => x.Id == id);
            Convocatoria objConvocatoria = await _context.Convocatorias.FirstOrDefaultAsync(x => x.IdConvocatoria == idConvocatoria);

            objConvocatoria.Abierto = !objConvocatoria.Abierto;
            objConvocatoria.IdUserUpdater = objUser.Id;
            objConvocatoria.UserUpdaterName = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);
            objConvocatoria.FechaActualizacion = DateTime.Now;

            _context.Convocatorias.Update(objConvocatoria);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task ToggleVisibility(ClaimsPrincipal user, dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = Globals.GetClaim("Id", user);
            string idConvocatoria = data.idConvocatoria;

            AspNetUser objUser = await _context.AspNetUsers.FirstOrDefaultAsync(x => x.Id == id);
            Convocatoria objConvocatoria = await _context.Convocatorias.FirstOrDefaultAsync(x => x.IdConvocatoria == idConvocatoria);

            objConvocatoria.Eliminado = !objConvocatoria.Eliminado;
            objConvocatoria.IdUserUpdater = objUser.Id;
            objConvocatoria.UserUpdaterName = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);
            objConvocatoria.FechaActualizacion = DateTime.Now;

            _context.Convocatorias.Update(objConvocatoria);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task Update(ClaimsPrincipal user, ActualizarConvocatoria.Root data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = Globals.GetClaim("Id", user);
            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            Convocatoria objConvocatoria = await _context.Convocatorias.AsNoTracking().FirstOrDefaultAsync(x => x.IdConvocatoria == data.idConvocatoria);

            objConvocatoria.Titulo              = data.titulo;
            objConvocatoria.Texto               = data.body;
            objConvocatoria.FechaActualizacion  = DateTime.Now;
            objConvocatoria.IdUserUpdater       = id;
            objConvocatoria.UserUpdaterName     = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);

            if(!String.IsNullOrEmpty(data.portada))
            {
                //removemos la portada anterior
                string previousPath = Path.Combine(objConvocatoria.PortadaPath);
                if(File.Exists(previousPath)) { File.Delete(previousPath); }

                //añadimos la nueva portada
                string newPath = Path.Combine("Public", "Convocatorias", "Portadas", $"{Guid.NewGuid().ToString()}.png");
                byte[] portadaBytes = Convert.FromBase64String(data.portada.Split(",")[1]);

                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    stream.Write(portadaBytes, 0, portadaBytes.Length);
                    stream.Flush();
                }

                objConvocatoria.PortadaPath = newPath;
            }

            List<string> lstFiles = JsonConvert.DeserializeObject<List<string>>(objConvocatoria.FilesArray);

            List<string> deletedFiles   = lstFiles.Where(x => !data.files.Select(x => x.name).Contains(x.Split("\\")[3])).ToList();
            lstFiles                    = lstFiles.Where(x => data.files.Select(x => x.name).Contains(x.Split("\\")[3])).ToList();
            
            //Creación de archivos añadidos
            data.files.ForEach(x => {
                if(x.base64 != "exist")
                {
                    byte[] bytes = Convert.FromBase64String(x.base64.Split(',')[1]);
                    string path = Path.Combine("Public", "Convocatorias", "Recursos", x.name);
                    using(FileStream stream = new(path, FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }

                    lstFiles.Add(path);
                }
            });

            objConvocatoria.FilesArray = JsonConvert.SerializeObject(lstFiles);

            //remover archivos eliminados
            deletedFiles.ForEach(x => {
                string path = Path.Combine(x);
                if(File.Exists(path)){ File.Delete(path); }
            });

            _context.Convocatorias.Update(objConvocatoria);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task<dynamic> Watch(dynamic data)
        {
            string id = data.id;
            return await _context.Convocatorias
            .AsNoTracking()
            .Where(x => x.IdConvocatoria == id)
            .Select(x => new {
                x.Titulo,
                x.Texto,
                x.Abierto,
                x.PortadaPath,
                FechaCreacion = x.FechaCreacion.ToString("G"),
                FechaActualizacin = x.FechaActualizacion.ToString("G"),
                x.UserCreatorName,
                x.UserUpdaterName,
                x.Eliminado,
                FilesArray = JsonConvert.DeserializeObject<List<string>>(x.FilesArray)
            })
            .FirstOrDefaultAsync();
        } 
    }
}