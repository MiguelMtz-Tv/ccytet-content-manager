using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels.Req.Noticias
{
    public class CreateNoticiaReq
    {

        public class Root
        {
            public string titulo { get; set; }
            public string autor { get; set; }
            public string body { get; set; }
            public List<File> files { get; set; }
        }

        public class File
        {
            public string uid { get; set; }
            public string base64 { get; set; }
        }
    }
}