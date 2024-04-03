using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels.Res
{
    public class LoginRes
    {
        public string   Token         { get; set; }
        public string   Id            { get; set; }
        public string   Nombre        { get; set; }
        public DateTime Expiration    { get; set; }
        public DateTime Expires       { get; set; }
    }
}