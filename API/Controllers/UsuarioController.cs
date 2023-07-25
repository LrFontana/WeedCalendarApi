using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.ResponseDto;
using AutoMapper;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Models;
using System.Net;
using Core.Dtos.UsuarioDto;

namespace API.Controllers
{    

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //variables.        
        private readonly IUnidadDeTrabajo _unidadTrabajo;
        private ResponsePaginatorDto _responsePaginador;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioController> _logger;

        //Constructor.
        public UsuarioController(IUnidadDeTrabajo unidadTrabajo, ILogger<UsuarioController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;            
            _responsePaginador = new ResponsePaginatorDto();
        }

        //Metodos.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            
            _logger.LogInformation("Listado de Usuarios");

            //Query.
            var lista = await _unidadTrabajo.Usuario.ObtenerTodos();

            _responsePaginador.Resultado = lista;
            _responsePaginador.Mensaje = "Listado de Usuarios.";
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            

            //Devuelve una lista de tipo usuario.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        
        [HttpGet("{id}", Name = "GetUsuarioPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Usuario>> GetUsuarioPorId(int id)
        {
            
            //Verifica si el id que recibe es diferente a 0.
            if(id == 0){
                _logger.LogError("Debe de Enviar el ID ");
                _responsePaginador.Mensaje = "Debe de Enviar el ID";
                _responsePaginador.IsExitoso = false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Query.
            var comp = await _unidadTrabajo.Usuario.Obtenerprimero(u => u.Id == id); 

            //Verifica si existe ese id en la tabla
            if(comp==null){
                _logger.LogError("No existe ese ID");
                _responsePaginador.Mensaje ="No existe ese ID";
                _responsePaginador.IsExitoso = false;
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_responsePaginador);
            }           
            _responsePaginador.Resultado = comp;
            _responsePaginador.Mensaje = "Datos del Usuario." + comp.Id;   
            _responsePaginador.IsExitoso= true;         
            _responsePaginador.StatusCode = HttpStatusCode.OK;

            //Devuelve una lista de tipo usuario por id.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioDto usuarioDto){

            //Verifica si lo recibido por input no es nulo.
            if(usuarioDto == null){
                _responsePaginador.Mensaje = "La informacion ingresada es incorrecta";
                _responsePaginador.IsExitoso=false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica que cada una de las propiedades esten completas.
            //trabaja directamente con el modelo.
            if(!ModelState.IsValid){

                return BadRequest(ModelState);
            }
            
            //Query.
            var companiaExiste = await _unidadTrabajo.Usuario.Obtenerprimero(u => u.NombreUsuario.ToLower() == usuarioDto.NombreUsuario.ToLower());

            //Verifica si el nombre ya existe.
            if(companiaExiste != null){                
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre del Usuario ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //relaciona el objeto que contiene la clase usuarioDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //variable.
            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

            //Agrega un nuevo regristo en la tabla Compania.
            await _unidadTrabajo.Usuario.Agregar(usuario);
            //Graba el nuevo registro en la tabla.
            await _unidadTrabajo.Guardar();   
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Uusuario Guardado con EXITO!";
            _responsePaginador.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetUsuarioPorId", new {id=usuarioDto}, _responsePaginador); //El CreatedAtRoute() me garantiza que el status code que devuelve sea igual a 201 por ser un metodo de tipo POST.   
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Usuario>> PutUsuario(int id, [FromBody] UsuarioDto usuarioDto){

            //Verifica si el id que recibe del end point es igual al id que tiene compania.
            if(id != usuarioDto.Id)
            {
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El ID ingresado no es correcto.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica si todos los datos del objeto son validos.
            if(!ModelState.IsValid)
            {   
                return BadRequest(ModelState); 
            }

            //Verifica si el nombre existe.
            //Query.
            var usuarioExiste = await _unidadTrabajo.Usuario.Obtenerprimero(u => u.NombreUsuario.ToLower() == usuarioDto.NombreUsuario.ToLower()
                                                                              && u.Id != usuarioDto.Id);

            if(usuarioExiste != null){
                
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El nombre del Usuario ya existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }            

            //relaciona el objeto que contiene la clase companiaDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //variable.
            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

            //actualiza los datos.
            _unidadTrabajo.Usuario.Actualizar(usuario);

            //graba los datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Usuario Actualizado";
            _responsePaginador.StatusCode = HttpStatusCode.OK;

            //devuelve una lista de tipo usuario.
            return Ok(_responsePaginador);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUsuario(int id){

            //Query.(consulta en la base de datos si existe ese id)
            var usuario = await _unidadTrabajo.Usuario.Obtenerprimero(u => u.Id == id);
            
            //Si no encontro ningun registro retontar "No Encontrado".
            if(usuario == null){

                _responsePaginador.IsExitoso = false; 
                _responsePaginador.Mensaje = "Usuario No Encontrado";
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }

            //Si encuentra uno lo elimina
            _unidadTrabajo.Usuario.Remover(usuario);

            //Actualiza la base de datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Compañia Eliminada";
            _responsePaginador.StatusCode = HttpStatusCode.NoContent;

            return Ok(_responsePaginador);

        }

        
        [HttpPost("Register")]
        public async Task<ActionResult> Register(UsuarioDto usuario)
        {
            //Query.
            var response = await _unidadTrabajo.Usuario.Register(
                                new Usuario{
                                    NombreUsuario = usuario.NombreUsuario                                    
                                }, usuario.Password);

            
            //Response validation.
            if (response == -1)
            {
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Usuario Ya existe";
                return BadRequest(_responsePaginador);
            }

            if (response == -500)
            {
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "Error al crear el usuario";
                return BadRequest(_responsePaginador);
            }

            _responsePaginador.Mensaje = "Usuario Creado con Exito";
            _responsePaginador.Resultado = response;    
            
            return Ok(_responsePaginador);
        }   

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UsuarioDto usuario)
        {
            //Query.
            var userLogin = await _unidadTrabajo.Usuario.Login(usuario.NombreUsuario, usuario.Password);

            if (userLogin == "nouser")
            {
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "Usuario N o Encontrado";
                return BadRequest(_responsePaginador);
            }

            if (userLogin == "badpassword")
            {
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "Password Incorrecto";
                return BadRequest(_responsePaginador);
            }

            _responsePaginador.Resultado = userLogin;
            _responsePaginador.Mensaje = "Usuario Conectado";
            return Ok(_responsePaginador);
        }        

    }
}