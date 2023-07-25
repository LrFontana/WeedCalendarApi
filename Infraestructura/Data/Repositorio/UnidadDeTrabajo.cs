using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.Extensions.Configuration;

namespace Infraestructura.Data.Repositorio
{
    public class UnidadDeTrabajo : IUnidadDeTrabajo
    {

        //Variable
        private readonly ApplicationDbContext _db;       
        public IUsuarioRepositorio Usuario {get; private set;}
        public IPlantasRepositorio Plantas {get; private set;}
        public ISistemaRiegoRepositorio SistemaRiego {get; private set;}
        public INotificacionesRepositorio Notificaciones {get; private set;}

        //Constructor.
        public UnidadDeTrabajo(ApplicationDbContext db, IConfiguration configuration)
        {            
            _db = db;
            Usuario = new UsuarioRepositorio(db, configuration);
            Plantas = new PlantasRepositorio(db, configuration);
            SistemaRiego = new SistemaRiegoRepositorio(db, configuration);
            Notificaciones = new NotificacionesRepositorio(db, configuration);

        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}