using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class NotificacionesConfig : IEntityTypeConfiguration<Notificaciones>
    {
        public void Configure(EntityTypeBuilder<Notificaciones> builder)
        {
            //TODO PROPIEDADES.

            builder.Property(n => n.Id).IsRequired();
            builder.Property(n => n.TipoDeNotificacion).IsRequired().HasMaxLength(25);
            builder.Property(n => n.Mensaje).IsRequired().HasMaxLength(50);
            builder.Property(n => n.FechaHora).IsRequired();

            //TODO RELACIONES.
            builder.HasOne(n => n.Usuario).WithMany().HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Plantas).WithMany().HasForeignKey(p => p.PlantasId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.SistemaRiego).WithMany().HasForeignKey(p => p.SistemaRiegoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}