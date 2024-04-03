using System.ComponentModel.DataAnnotations;

namespace ccytet.Server.Models
{
    public class EstadoSituacionFinancieraArchivo
    {
        [Key]
        public string IdEstadoSituacionFinancieraArchivo                    { get; set; }
        public string Titulo                                                { get; set;}
        public string Path                                                  { get; set; }
        public bool Deleted                                                 { get; set; }
        public DateTime FechaCreacion                                       { get; set; }
        public DateTime? FechaEliminación                                   { get; set; }

        //RELATIONS
        public virtual EstadoSituacionFinanciera EstadoSituacionFinanciera  { get; set; }
        public string IdEstadoSituacionFinanciera                           { get; set; }
        
        public virtual AspNetUser UserCreator                               { get; set; }
        public string IdUserCreator                                         { get; set; }

        public virtual AspNetUser UserDeleter                               { get; set; }
        public string  IdUserDeleter                                        { get; set; }
    }
}