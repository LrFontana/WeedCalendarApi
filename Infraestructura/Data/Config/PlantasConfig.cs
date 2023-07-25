using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class PlantasConfig : IEntityTypeConfiguration<Plantas>
    {
        public void Configure(EntityTypeBuilder<Plantas> builder)
        {
            //TODO PROPIEDADES.

            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.NombrePlanta).IsRequired().HasMaxLength(30);
            builder.Property(p => p.VariedadPlanta).IsRequired().HasMaxLength(15);
            builder.Property(p => p.EstapaCrecimento).IsRequired();
            builder.Property(p => p.Notas).IsRequired();
            builder.Property(p => p.FechaSiembre).IsRequired();
            builder.Property(p => p.FechaCosecha).IsRequired();

            //TODO RELACIONES.
            builder.HasOne(p => p.Usuario).WithMany().HasForeignKey(p => p.UsuarioId);
        }
    }
}