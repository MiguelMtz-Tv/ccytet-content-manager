using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ccytet.Server.Models
{
    public class AspNetUser : IdentityUser
    {
        public string Nombre                                                        { get; set; }
        public string Apellidos                                                     { get; set; }
        public string Matricula                                                     { get; set; }

        //RELATIONS
        public virtual List<Noticia> NoticiasCreadas                                { get; set; }
        public virtual List<Noticia> NoticiasActualizadas                           { get; set; }

        public virtual List<Convocatoria> ConvocatoriasCreadas                      { get; set; }
        public virtual List<Convocatoria> ConvocatoriasActualizadas                 { get; set; }

        public virtual List<EstadoSituacionFinanciera> ESFCreados                   { get; set; }
        public virtual List<EstadoSituacionFinanciera> ESFActualizados              { get; set; }
    }
}