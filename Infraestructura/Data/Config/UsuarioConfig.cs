using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // TODO Propiedades.

            builder.Property(u => u.Id).IsRequired();
            builder.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(20);
            builder.Property(u => u.Mail).IsRequired().HasMaxLength(25);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(25);
        }
    }

}