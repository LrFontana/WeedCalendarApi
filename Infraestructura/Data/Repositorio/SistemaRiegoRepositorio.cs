using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.Extensions.Configuration;

namespace Infraestructura.Data.Repositorio
{
    public class SistemaRiegoRepositorio : RepositorioGlobal<SistemaRiego>, ISistemaRiegoRepositorio
    {

        //Variable.
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public SistemaRiegoRepositorio(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        public void Actualizar(SistemaRiego sistemaRiego)
        {
            //Query.
            var queryActualizarPsistemaRiego = _db.TblSistemaRiego.FirstOrDefault(sR => sR.Id == sistemaRiego.Id);
            
            //Verifica si query es diferente a null, y si lo es actualiza los datos y guarda los cambios.
            if(queryActualizarPsistemaRiego != null)
            {
                queryActualizarPsistemaRiego.CantidadAgua = sistemaRiego.CantidadAgua;
                queryActualizarPsistemaRiego.Descripcion = sistemaRiego.Descripcion;
                queryActualizarPsistemaRiego.TipoSistema = sistemaRiego.TipoSistema;
                queryActualizarPsistemaRiego.ProporcionNutrientes = sistemaRiego.ProporcionNutrientes;
                queryActualizarPsistemaRiego.UsuarioId = sistemaRiego.UsuarioId;
                queryActualizarPsistemaRiego.PlantasId = sistemaRiego.PlantasId;                
                _db.SaveChanges();
            }
        }

        public List<DateTime> DiasDeRiego(DateTime fecha, int diasRiego)
        {
                // Lista para almacenar las fechas de riego intermitente
            List<DateTime> fechasRiego = new List<DateTime>();

            // Verificar que la fecha de siembra sea válida
            if (fecha == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de siembra no es válida.");
            }

            // Verificar que la cantidad de días de riego sea válida
            if (diasRiego <= 0)
            {
                throw new ArgumentException("La cantidad de días de riego debe ser mayor que cero.");
            }

            // Agregar la fecha de siembra como la primera fecha de riego
            fechasRiego.Add(fecha);

            // Calcular las fechas de riego intermitente
            for (int i = 1; i < diasRiego; i++)
            {
                // Agregar la fecha de riego intermitente sumando i * cantidadDiasRiego días a la fecha de siembra
                DateTime fechaRiego = fecha.AddDays(i * diasRiego);
                fechasRiego.Add(fechaRiego);
            }

            return fechasRiego;
        }
    }
}