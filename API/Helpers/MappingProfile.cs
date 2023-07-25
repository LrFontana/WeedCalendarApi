using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.NotificacionesDto;
using Core.Dtos.PlatasDto;
using Core.Dtos.SistemaRiegoDto;
using Core.Dtos.UsuarioDto;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();

            CreateMap<Plantas, PlantasUpsertDto>().ReverseMap();
            CreateMap<Plantas, PlantasReadDto>().ForMember(p => p.usuarioNombre, m => m.MapFrom(u => u.Usuario.NombreUsuario));

            CreateMap<SistemaRiego, SistemaRiegoUpsertDto>().ReverseMap();
            CreateMap<SistemaRiego, SistemaRiegoReadDto>().ForMember(sR => sR.usuarioNombre, m => m.MapFrom(u => u.Usuario.NombreUsuario))
                                                          .ForMember(sR => sR.plantasNombre, m => m.MapFrom(p => p.Plantas.NombrePlanta));

            CreateMap<Notificaciones, NotificacionesUpsertDto>().ReverseMap();
            CreateMap<Notificaciones, NotificacionesReadDto>().ForMember(n => n.usuarioNombre, m => m.MapFrom(u => u.Usuario.NombreUsuario))
                                                              .ForMember(n => n.plantasNombre, m => m.MapFrom(p => p.Plantas.NombrePlanta))
                                                              .ForMember(n => n.sistemaRiegoNombre, m => m.MapFrom(sR => sR.SistemaRiego.Descripcion));
        }
    }
}