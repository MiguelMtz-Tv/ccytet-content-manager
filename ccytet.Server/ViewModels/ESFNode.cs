using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccytet.Server.ViewModels
{
    public class ESFNode
    {
        public int Level { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public List<ESFNode> Children {get; set;}
    }
}