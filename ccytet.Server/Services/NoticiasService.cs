using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.ViewModels;
using ccytet.Server.ViewModels.Req.Noticias;
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
                UserCreatorName = objUser.Nombre + " " +objUser.Apellidos,
                UserUpdaterName = objUser.Nombre + " " +objUser.Apellidos,
                IdUserUpdater = id
            };

            List<CreateNoticiaReq.File> files = data.files;
            List<string> noticiaImages = new();

            foreach(CreateNoticiaReq.File file in files )
            {
                string imagePath = Path.Combine("Public","Noticias");
                string fileName = $"{Guid.NewGuid().ToString()}.png";

                string storagePath = Path.Combine(imagePath, fileName);

                byte[] bytes = Convert.FromBase64String(file.base64.Replace("data:image/png;base64,", ""));

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
                    item.Autor
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
            IQueryable<Noticia> rows = _context.Noticias.AsNoTracking().OrderBy(x => x.FechaCreacion).Where(x => !x.Eliminado);

            if(!String.IsNullOrEmpty(data.search.Value))
            {
                string arg = data.search.Value;
                rows = rows.Where(x => x.Titulo.Contains(arg) || x.Autor.Contains(arg) || x.Texto.Contains(arg));
            }

            query = rows.ProjectTo<NoticiaViewModel>(_mapper.ConfigurationProvider);
            return query;
        }
       
        public async Task<dynamic> Watch(dynamic data)
        {
            string id = data.id;
            Noticia objNoticia = await _context.Noticias.AsNoTracking().Where(x => x.IdNoticia == id).FirstOrDefaultAsync();
            dynamic noticia = new {
                view = objNoticia,
                images = JsonConvert.DeserializeObject<List<string>>(objNoticia.ImagesArray)
            };

            return noticia;
        }

        public async Task ToggleStatus(dynamic data)
        {
            var loginTransaction = await _context.Database.BeginTransactionAsync();

            string id = data.id;
            Noticia noticia= await _context.Noticias.Where(x => x.IdNoticia == id).FirstOrDefaultAsync();
            noticia.Eliminado = !noticia.Eliminado;

            _context.Noticias.Update(noticia);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }

        public static string ReduceText(string text)
        {
            return text.Substring(0, 30) + "...";
        }
    }
}