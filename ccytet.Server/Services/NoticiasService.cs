using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.ViewModels;
using ccytet.Server.ViewModels.Req.Noticias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Workcube.Libraries;

namespace ccytet.Server.Services
{
    public class NoticiasService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public NoticiasService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Store(ClaimsPrincipal user, CreateNoticiaReq.Root data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = Globals.GetClaim("Id", user);
            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            
            Noticia objNoticia = new()
            {
                IdNoticia = Guid.NewGuid().ToString(),
                Titulo = data.titulo,
                Autor = !String.IsNullOrEmpty(data.autor) ? data.autor : "Autor anonimo",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Texto = data.body,
                Eliminado = false,
                IdUserCreator = id,
                UserCreatorName = string.Format("{0} {1}", objUser.Nombre, objUser.Apellidos),
                UserUpdaterName = string.Format("{0} {1}", objUser.Nombre, objUser.Apellidos),
                IdUserUpdater = id
            };

            List<CreateNoticiaReq.File> files = data.files;
            List<string> noticiaImages = new();

            foreach(CreateNoticiaReq.File file in files )
            {
                string imagePath = Path.Combine("Public","Noticias");
                string fileName = $"{Guid.NewGuid().ToString()}.png";

                string storagePath = Path.Combine(imagePath, fileName);

                byte[] bytes = Convert.FromBase64String(file.base64.Split(',')[1]);

                using (FileStream stream = new (storagePath, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }

                noticiaImages.Add(storagePath);
            }

            objNoticia.ImagesArray = JsonConvert.SerializeObject(noticiaImages);
            objNoticia.Portada = noticiaImages.First();
            
            await _context.Noticias.AddAsync(objNoticia);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task<dynamic> DataSource(dynamic data)
        {
            IQueryable<NoticiaViewModel> lstData = DataSourceExpression(data);

            DataSourceBuilder<NoticiaViewModel> objDataSourceBuilder = new DataSourceBuilder<NoticiaViewModel>(data, lstData);
            var objDataBuilder = await objDataSourceBuilder.build();

            // CONSTRUCCION RETORNO DE DATOS
            List<NoticiaViewModel> lstOriginal = objDataBuilder.rows;
            List<dynamic>  lstRows = new();

            lstOriginal.ForEach(item => 
                lstRows.Add(new {
                    item.IdNoticia,
                    item.Titulo,
                    FechaCreacion = item.FechaCreacionNatural,
                    FechaActualizacion = item.FechaActualizacionNatural,
                    item.UserCreatorName,
                    item.UserUpdaterName,
                    item.Portada,
                    item.Autor,
                    item.Eliminado
                })
            );

            var objResult = new
            {
                rows    = lstRows,
                count   = objDataBuilder.count,
                length  = objDataBuilder.length,
                pages   = objDataBuilder.pages,
                page    = objDataBuilder.page,
            };

            return objResult;
        }
       
        public IQueryable<NoticiaViewModel> DataSourceExpression(dynamic data)
        {
            IQueryable<NoticiaViewModel> query;
            IQueryable<Noticia> rows = _context.Noticias.AsNoTracking().OrderByDescending(x => x.FechaCreacion);

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
                rows = rows.Where(x => x.Titulo.Contains(arg) || x.Autor.Contains(arg) || x.Texto.Contains(arg));
            }

            query = rows.ProjectTo<NoticiaViewModel>(_mapper.ConfigurationProvider);
            return query;
        }
       
        public async Task Update(ActualizarNoticiaReq.Root data, ClaimsPrincipal user)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            Noticia objNoticia = await _context.Noticias.FindAsync(data.IdNoticia);
            
            string id = Globals.GetClaim("Id", user);
            AspNetUser objUser = await _context.AspNetUsers.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

            objNoticia.Titulo                = data.titulo;
            objNoticia.Texto                 = data.body;
            objNoticia.Autor                 = !String.IsNullOrEmpty(data.autor) ? data.autor : "Autor anonimo";  
            objNoticia.FechaActualizacion    = DateTime.Now;
            objNoticia.IdUserUpdater         = objUser.Id;
            objNoticia.UserUpdaterName       =  string.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);

            List<string> lstImagenes        = JsonConvert.DeserializeObject<List<string>>(objNoticia.ImagesArray);
            List<string> removedImages      = lstImagenes.Where(x => !data.files.Select(x => x.uid).Contains(GetImageName(x))).ToList();

            lstImagenes = lstImagenes.Where(x => data.files.Select(x => x.uid).Contains(GetImageName(x))).ToList(); // remove images removed for users

            data.files.ForEach(x =>
            {
                if(x.base64 != "exist")
                {
                    string imagePath    = Path.Combine("Public","Noticias");
                    string fileName     = $"{Guid.NewGuid().ToString()}.png";
                    string storagePath  = Path.Combine(imagePath, fileName);
                    byte[] bytes        = Convert.FromBase64String(x.base64.Split(',')[1]);

                    using (FileStream stream = new (storagePath, FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }

                    lstImagenes.Add(storagePath);
                }
            });

            removedImages.ForEach(x => //delete removed images
            {
                if(File.Exists(Path.Combine(x)))
                {
                    File.Delete(Path.Combine(x));
                }
            });

            objNoticia.Portada  = lstImagenes.FirstOrDefault();
            objNoticia.ImagesArray = JsonConvert.SerializeObject(lstImagenes);

            _context.Noticias.Update(objNoticia);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public async Task<dynamic> Watch(dynamic data)
        {
            string id = data.id;
            Noticia objNoticia = await _context.Noticias.AsNoTracking().Where(x => x.IdNoticia == id).FirstOrDefaultAsync();
            dynamic noticia = new {
                view = new 
                {
                    IdNoticia           = objNoticia.IdNoticia,
                    Titulo              = objNoticia.Titulo,
                    Texto               = objNoticia.Texto,
                    Autor               = objNoticia.Autor,
                    FechaCreacion       = objNoticia.FechaCreacion.ToString("dd/MM/yyyy hh:mm tt"),
                    FechaActualizacion  = objNoticia.FechaActualizacion.ToString("dd/MM/yyyy hh:mm tt"),
                    Eliminado           = objNoticia.Eliminado,
                    UserCreator         = objNoticia.UserCreator,
                    UserUpdater         = objNoticia.UserUpdater
                },
                images = JsonConvert.DeserializeObject<List<string>>(objNoticia.ImagesArray)
            };

            return noticia;
        }

        public async Task ToggleVisibility(ClaimsPrincipal user, dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = data.idNoticia;
            string  idAspNetUser = Globals.GetClaim("Id", user);

            AspNetUser objUser = await _context.AspNetUsers.FirstOrDefaultAsync(x => x.Id == idAspNetUser);
            Noticia noticia= await _context.Noticias.Where(x => x.IdNoticia == id).FirstOrDefaultAsync();
            
            noticia.Eliminado = !noticia.Eliminado;
            noticia.IdUserUpdater = objUser.Id;
            noticia.UserUpdaterName = String.Format("{0} {1}", objUser.Nombre, objUser.Apellidos);
            noticia.FechaActualizacion = DateTime.Now;

            _context.Noticias.Update(noticia);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        private static string ReduceText(string text)
        {
            return text.Substring(0, 30) + "...";
        }

        private static string GetImageName(string name)
        {
            return name.Split("\\")[2];
        }
    }
}