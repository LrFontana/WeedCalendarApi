using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.Extensions.Configuration;

namespace Infraestructura.Data.Repositorio
{
    public class NotificacionesRepositorio : RepositorioGlobal<Notificaciones>, INotificacionesRepositorio
    {

        //Variable.
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public NotificacionesRepositorio(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        public void Actualizar(Notificaciones notificaciones)
        {
            //Query.
            var queryActualizarNotificaciones = _db.TblNotificaciones.FirstOrDefault(n=>n.Id == notificaciones.Id);
            
            //Verifica si query es diferente a null, y si lo es actualiza los datos y guarda los cambios.
            if(queryActualizarNotificaciones != null)
            {
                queryActualizarNotificaciones.FechaHora = notificaciones.FechaHora;                
                queryActualizarNotificaciones.Mensaje = notificaciones.Mensaje;
                queryActualizarNotificaciones.TipoDeNotificacion = notificaciones.TipoDeNotificacion;
                queryActualizarNotificaciones.UsuarioId = notificaciones.UsuarioId;
                queryActualizarNotificaciones.PlantasId = notificaciones.PlantasId;
                queryActualizarNotificaciones.SistemaRiegoId = notificaciones.SistemaRiegoId;
                _db.SaveChanges();
            }
        }
    }
}