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
        public DateTime Desde                                           { get; set; }
        public DateTime Hasta                                           { get; set; }
        public string Titulo                                            { get; set; }

        //RELATIONS
        public virtual AspNetUser UserCreator                           { get; set; }
        public string IdUserCreator                                     { get; set; }
        
        public virtual AspNetUser UserUpdater                           { get; set; }
        public string IdUserUpdater                                     { get; set; }
    }
}