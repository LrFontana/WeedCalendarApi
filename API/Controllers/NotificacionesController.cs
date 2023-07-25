using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.NotificacionesDto;
using Core.Dtos.ResponseDto;
using Core.Especificaciones;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacionesController : ControllerBase
    {
        //variables.        
        private readonly IUnidadDeTrabajo _unidadTrabajo;
        private ResponsePaginatorDto _responsePaginador;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificacionesController> _logger;

        //Constructor.
        public NotificacionesController(IUnidadDeTrabajo unidadTrabajo, ILogger<NotificacionesController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;            
            _responsePaginador = new ResponsePaginatorDto();
        }
        
        //Metodos.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Notificaciones>>> GetNotificaciones([FromQuery] Parametros parametros)
        {
            
            _logger.LogInformation("Listado de Notificaciones");

            //Query.
            var lista = await _unidadTrabajo.Notificaciones.ObtenerTodosPaginado(
                                                                          parametros, 
                                                                          incluirPropiedades:"Usuario, Plantas, SistemaRiego",
                                                                          orderBy: n => n.OrderBy(n=>n.Plantas).ThenBy(n=>n.TipoDeNotificacion)); // retorna la lista de notificaciones y tambien cluye todos los datos de la plantas.
            
            //mapeo.
            _responsePaginador.TotalPaginas = lista.MetaData.TotalPages;
            _responsePaginador.TotalRegistros = lista.MetaData.TotalCount;
            _responsePaginador.PageSize = lista.MetaData.PageSize;
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<Notificaciones>, IEnumerable<NotificacionesReadDto>>(lista);         
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de Notificaciones.";
            

            //Devuelve una lista de tipo empleado.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }


        [HttpGet("{id}", Name = "GetNotificacionesPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Notificaciones>> GetNotificacionesPorId(int id){
            
            //Verifica si el id que recibe es igual a 0.
            if(id == 0){
                _logger.LogError("Debe de Enviar el ID ");
                _responsePaginador.Mensaje = "Debe de Enviar el ID";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                _responsePaginador.IsExitoso = false;                
                return BadRequest(_responsePaginador);
            }

            //Query.            
            var noti = await _unidadTrabajo.Notificaciones.Obtenerprimero(n => n.Id == id, incluirPropiedades: "Plantas"); // retorna la lista de notificaciones y tambien cluye todos los datos de la plantas.

            //Verifica si existe ese id en la tabla
            if(noti==null){
                _logger.LogError("No existe ese ID");
                _responsePaginador.Mensaje ="No existe ese ID";
                _responsePaginador.IsExitoso = false;
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_responsePaginador);
            }          

            //mapeo. 
            _responsePaginador.Resultado = _mapper.Map<Notificaciones, NotificacionesReadDto>(noti);
            //si todo sale bien.
            _responsePaginador.Mensaje = "Datos de la Notificacion." + noti.Id;   
            _responsePaginador.IsExitoso= true;     
            _responsePaginador.StatusCode = HttpStatusCode.OK;    

            //Devuelve una lista de tipo notificaciones por id.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        [HttpGet]
        [Route("NotificacionesPorPlantas/{plantasId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<ActionResult<IEnumerable<NotificacionesReadDto>>> GetNotificacionesPorPlanta(int plantasId)
        { 
            _logger.LogInformation("Listado de Notificaciones Por Planta");   

            //Query.            
            var notiLista = await _unidadTrabajo.Notificaciones.ObtenerTodos(n=>n.PlantasId == plantasId, incluirPropiedades: "Plantas");// retorna una lista de notificaciones filtadas por el parametro id.
            
            //mapeo.
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<Notificaciones>, IEnumerable<NotificacionesReadDto>>(notiLista);
            //si todo sale bien.
            _responsePaginador.IsExitoso = true;
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de Notificaciones por Planta.";            
            return Ok(_responsePaginador);
        }

        [HttpGet]
        [Route("NotificacionesPorSistemaRiego/{sistemaRiegoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<ActionResult<IEnumerable<NotificacionesReadDto>>> GetNotificacionesPorsistemaRiego(int sistemaRiegoId)
        { 
            _logger.LogInformation("Listado de Notificaciones Por Sistema Riego");   

            //Query.            
            var notiLista = await _unidadTrabajo.Notificaciones.ObtenerTodos(n=>n.SistemaRiegoId == sistemaRiegoId, incluirPropiedades: "sistema Riego");// retorna una lista de notificaciones filtadas por el parametro id.
            
            //mapeo.
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<Notificaciones>, IEnumerable<NotificacionesReadDto>>(notiLista);
            //si todo sale bien.
            _responsePaginador.IsExitoso = true;
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de Notificaciones por Sistema Riego.";            
            return Ok(_responsePaginador);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<Notificaciones>> PostNotificaciones([FromBody] NotificacionesUpsertDto notificacionesDto){

            //Verifica si lo recibido por input no es nulo.
            if(notificacionesDto == null){
                _responsePaginador.Mensaje = "La informacion ingresada es incorrecta";
                _responsePaginador.IsExitoso=false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica que cada una de las propiedades esten completas.
            //trabaja directamente con el modelo.
            if(!ModelState.IsValid){
                _responsePaginador.Mensaje = "La informacion ingresada es incorrecta";
                _responsePaginador.IsExitoso=false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
                
            }
            
            //Query.
            var notificacionesExiste = await _unidadTrabajo.Notificaciones.Obtenerprimero(n => n.TipoDeNotificacion.ToLower() == notificacionesDto.TipoDeNotificacion.ToLower() &&
                                                                              n.Mensaje.ToLower() == notificacionesDto.Mensaje.ToLower());

            //Verifica si el nombre ya existe.
            if(notificacionesExiste != null){
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre de la Notificacion ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //relaciona el objeto que contiene la clase notificacionesDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //mapeo.
            Notificaciones notificacion = _mapper.Map<Notificaciones>(notificacionesDto);

            //Agrega un nuevo regristo en la tabla notificaciones.
            await _unidadTrabajo.Notificaciones.Agregar(notificacion);
            //Graba el nuevo registro en la tabla.
            await _unidadTrabajo.Guardar(); 

            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Notificacion  Actualizado con EXITO!";
            _responsePaginador.StatusCode = HttpStatusCode.Created;
            _responsePaginador.Resultado = notificacion;
            return CreatedAtRoute("GetNotificacionesPorId", new {id=notificacionesDto}, _responsePaginador); //El CreatedAtRoute() me garantiza que el status code que devuelve sea igual a 201 por ser un metodo de tipo POST.   
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Notificaciones>> PutNotificaciones(int id, [FromBody] NotificacionesUpsertDto notificacionesDto){

            //Verifica si el id que recibe del end point es igual al id que tiene usuario.
            if(id != notificacionesDto.Id){

                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El ID ingresado no es correcto.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica si todos los datos del objeto son validos.
            if(!ModelState.IsValid)
            {   
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre de la Notificacion ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //Query.
            var notificacionExiste = await _unidadTrabajo.Notificaciones.Obtenerprimero(n => n.TipoDeNotificacion.ToLower() == notificacionesDto.TipoDeNotificacion.ToLower()
                                                                             && n.Mensaje.ToLower() == notificacionesDto.Mensaje.ToLower()
                                                                             && n.Id != notificacionesDto.Id);

            //Verifica si el nombre, apellido y id ya existe.
            if(notificacionExiste != null){

                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El nombre de la Notificacion ya existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }            

            //relaciona el objeto que contiene la clase notificacionesDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //mapeo.
            Notificaciones notificacion = _mapper.Map<Notificaciones>(notificacionesDto);

            //actualiza los datos.
            _unidadTrabajo.Notificaciones.Actualizar(notificacion);

            //graba los datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Empleado Guardado con EXITO";
            _responsePaginador.StatusCode = HttpStatusCode.NoContent;

            //devuelve una lista de tipo notificacion.
            return Ok(_responsePaginador);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteNotificacion(int id){

            //Query.(consulta en la base de datos si existe ese id)
            var notificacion = await _unidadTrabajo.Notificaciones.Obtenerprimero(n=> n.Id == id);
            
            //Si no encontro ningun registro retontar "No Encontrado".
            if(notificacion == null){

                _responsePaginador.IsExitoso = false; 
                _responsePaginador.Mensaje = "Empleado No Encontrado";
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_responsePaginador);
            }

            //Si encuentra uno lo elimina
           _unidadTrabajo.Notificaciones.Remover(notificacion);

            //Actualiza la base de datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Notificacion Eliminado";
            _responsePaginador.StatusCode = HttpStatusCode.NoContent;

            return Ok(_responsePaginador);
        }
    }
}