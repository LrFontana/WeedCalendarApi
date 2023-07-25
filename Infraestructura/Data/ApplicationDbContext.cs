using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Infraestructura.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data
{
    public class ApplicationDbContext : DbContext
    {

        //Constructor.
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //Relaciones de los modelos con las Tablas en la base de datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfig());
            modelBuilder.ApplyConfiguration(new PlantasConfig());
            modelBuilder.ApplyConfiguration(new SistemaRiegoConfig());
            modelBuilder.ApplyConfiguration(new NotificacionesConfig());
        }

        //Variables.
        public DbSet<Usuario> TblUsuario { get; set; }
        public DbSet<Plantas> TblPlantas { get; set; }
        public DbSet<SistemaRiego> TblSistemaRiego { get; set; }
        public DbSet<Notificaciones> TblNotificaciones { get; set; }

    }     
        
}