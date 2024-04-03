using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels.Req.Auth
{
    public class LoginReq
    {
        public string matricula { get; set; }
        public string password  { get; set; }
    }
}