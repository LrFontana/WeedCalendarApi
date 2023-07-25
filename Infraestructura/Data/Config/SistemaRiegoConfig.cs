using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class SistemaRiegoConfig : IEntityTypeConfiguration<SistemaRiego>
    {
        public void Configure(EntityTypeBuilder<SistemaRiego> builder)
        {
            //TODO PROPIEDADES.

            builder.Property(sR => sR.Id).IsRequired();
            builder.Property(sR => sR.CantidadAgua).IsRequired();
            builder.Property(sR => sR.Descripcion).IsRequired().HasMaxLength(300);
            builder.Property(sR => sR.TipoSistema).IsRequired().HasMaxLength(15);
            builder.Property(sR => sR.ProporcionNutrientes).IsRequired().HasMaxLength(30);

            
            //TODO RELACIONES.
            builder.HasOne(sR => sR.Usuario).WithMany().HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(sR => sR.Plantas).WithMany().HasForeignKey(p => p.PlantasId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}