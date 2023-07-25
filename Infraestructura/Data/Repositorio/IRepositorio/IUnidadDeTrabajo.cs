using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IUnidadDeTrabajo : IDisposable
    {
        IUsuarioRepositorio Usuario {get; }
        IPlantasRepositorio Plantas {get; }
        ISistemaRiegoRepositorio SistemaRiego {get; }
        INotificacionesRepositorio Notificaciones {get; }

        Task Guardar();
    }
}