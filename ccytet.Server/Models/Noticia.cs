using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.Models
{
    public class Noticia
    {
        [Key]
        public string IdNoticia                             { get; set; }
        public string Titulo                                { get; set; }
        public string Texto                                 { get; set; }
        public string Autor                                 { get; set; }
        public DateTime FechaCreacion                       { get; set; }
        public DateTime FechaActualizacion                  { get; set; }
        public string imagesArray                           { get; set; }

        public bool Eliminado                               { get; set; }

        //RELATIONS
        public virtual AspNetUser UserCreator               { get; set; }
        public string IdUserCreator                         { get; set; }

        public virtual AspNetUser UserUpdater               { get; set; }
        public string IdUserUpdater                         { get; set; }
    }
}