using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.Models
{
    public class EstadoSituacionFinanciera
    {
        [Key]
        public string IdEstadoSituacionFinanciera                       { get; set; }
        public DateTime Periodo                                         { get; set; }

        //RELATIONS
        public virtual AspNetUser UserCreator                           { get; set; }
        public string IdUserCreator                                     { get; set; }
        
        public virtual AspNetUser UserUpdater                           { get; set; }
        public string IdUserUpdater                                     { get; set; }

        public virtual List<EstadoSituacionFinancieraArchivo> EstadoSituacionFinancieraArchivos { get; set; }
    }
}