using System.ComponentModel.DataAnnotations;

namespace ccytet.Server.Models
{
    public class Convocatoria
    {
        [Key]
        public string IdConvocatoria                                    { get; set; }
        public string PortadaPath                                       { get; set; }
        public string Titulo                                            { get; set; }
        public string Autor                                             { get; set; }
        public string Texto                                             { get; set; }
        public DateTime FechaCreacion                                   { get; set; }
        public DateTime FechaActualizacion                              { get; set; }

        public bool Eliminado                                           { get; set; }
        public bool Abierto                                             { get; set; }

        //RELATIONS
        public virtual AspNetUser UserCreator                           { get; set; }
        public string IdUserCreator                                     { get; set; }

        public virtual AspNetUser UserUpdater                           { get; set; }
        public string IdUserUpdater                                     { get; set; }

    }
}