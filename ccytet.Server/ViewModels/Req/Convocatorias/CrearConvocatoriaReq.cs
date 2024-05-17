using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels.Req.Convocatorias
{
    public class CrearConvocatoriaReq
    {
        public class Root
        {
            public string titulo    { get; set; }
            public string body      { get; set; }
            public string portada   { get; set; }
            public List<File> files { get; set; }
        }

        public class File
        {
            public string uid       { get; set; }
            public string extension { get; set; }
            public string name      { get; set; }
            public string base64    { get; set; }
        }
    }
}