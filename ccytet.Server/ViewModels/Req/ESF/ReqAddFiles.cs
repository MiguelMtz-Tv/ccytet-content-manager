using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels.Req.ESF
{
    public class ReqAddFiles
    {
        public string idEstadoSituacionFinanciera { get; set; }
        public List<File> files { get; set; }

        public class File
        {
            public string name { get; set; }
            public string base64 { get; set; }
        }
    }
}