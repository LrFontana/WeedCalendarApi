using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Usuario
    {  
        //Propiedades. 
        
        [Key]       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]  
        public int Id { get; set; } 

        [Required(ErrorMessage = "El Nombre de Usuario es Requrido.")]
        [MaxLength(20, ErrorMessage = "El Nombre de Usuario no puede ser mayaor a 20 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La Contrasela es Requerida")]
        [MaxLength(25, ErrorMessage = "La Contrasela no puede exceder los 25 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El Mail es Requerido")]
        [MaxLength(25, ErrorMessage = "El Mail No puede exceder los 25 caracteres")]
        public string Mail { get; set; }

        
        // Para generar la contraseña encriptada.
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }       
        
        
    }
}