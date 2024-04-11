using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ccytet.Server.Models;
using ccytet.Server.ViewModels;

namespace ccytet.Server.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Noticia, NoticiaViewModel>();
        }
    }
}