using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels
{
    public class ConvocatoriaViewModel
    {
        public string IdConvocatoria                                    { get; set; }
        public string PortadaPath                                       { get; set; }
        public string Titulo                                            { get; set; }
        public string Autor                                             { get; set; }
        public string Texto                                             { get; set; }
        public string FilesArray                                        { get; set; }
        public DateTime FechaCreacion                                   { get; set; }
        public string FechaCreacionNatural                              => FechaCreacion.ToString("dd/MM/yyyy hh:mm tt");
        public DateTime FechaActualizacion                              { get; set; }
        public string FechaActualizacionNatural                         => FechaActualizacion.ToString("dd/MM/yyyy hh:mm tt");

        public bool Eliminado                                           { get; set; }
        public bool Abierto                                             { get; set; }

        //RELATIONS
        public string IdUserCreator                                     { get; set; }
        public string UserCreatorName                                   { get; set; }
        public string IdUserUpdater                                     { get; set; }
        public string UserUpdaterName                                   { get; set; }
    }
}