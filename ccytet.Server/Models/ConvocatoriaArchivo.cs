using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.Models
{
    public class ConvocatoriaArchivo
    {
        [Key]
        public string IdConvocatoriaArchivo         { get; set; }
        public string ConvocatoriaPath              { get; set; }

        //RELATIONS
        public virtual Convocatoria Convocatoria    { get; set; }
        public string IdConvocatoria                { get; set; }
    }
}