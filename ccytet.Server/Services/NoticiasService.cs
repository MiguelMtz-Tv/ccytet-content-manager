using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.ViewModels.Req.Noticias;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Workcube.Libraries;

namespace ccytet.Server.Services
{
    public class NoticiasService
    {
        private readonly DataContext _context;
        public NoticiasService(DataContext context)
        {
            _context = context;
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
                Autor = data.autor,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Texto = data.body,
                Eliminado = false,
                IdUserCreator = id,
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

            objNoticia.imagesArray = JsonConvert.SerializeObject(noticiaImages);
            
            await _context.Noticias.AddAsync(objNoticia);
            await _context.SaveChangesAsync();
            await loginTransaction.CommitAsync();
        }
    }
}