using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Core.Especificaciones;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infraestructura.Data.Repositorio
{
    public class UsuarioRepositorio : RepositorioGlobal<Usuario>, IUsuarioRepositorio
    {
        //Variable.
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public UsuarioRepositorio(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }
        
        public void Actualizar(Usuario usuario)
        {
            //Query.
            var queryActualizarUsuario = _db.TblUsuario.FirstOrDefault(u => u.Id == usuario.Id);
            
            //Verifica si query es diferente a null, y si lo es actualiza los datos y guarda los cambios.
            if(queryActualizarUsuario != null)
            {
                queryActualizarUsuario.NombreUsuario = usuario.NombreUsuario;
                queryActualizarUsuario.Password = usuario.Password;
                queryActualizarUsuario.Mail = usuario.Mail;                
                _db.SaveChanges();
            }
        }
        public async Task<string> Login(string userName, string password)
        {
            //Query.
            var user = await _db.TblUsuario.FirstOrDefaultAsync(u=>u.NombreUsuario.ToLower().Equals(userName.ToLower())); // chequea si el usuario existe.

            if (user == null)
            {
                return "nouser"; // usuario no encontrado
            }
            else if (!VerifyPassHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return "badpassword"; // password incorrecta.
            }
            else
            {
                return CreatToken(user);
            }
        }

        private bool VerifyPassHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false; // Credenciales inválidas
                    }
                }

                return true; // Credenciales válidas
            }
        }

        public async Task<int> Register(Usuario user, string password)
        {
            try
            {
                //Query.
                if (await UserExist(user.NombreUsuario))
                {
                    return -1;
                }

                CreatPassHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _db.TblUsuario.AddAsync(user);
                await _db.SaveChangesAsync();
                return user.Id;
            }
            catch (System.Exception)
            {
                
                return -500;
            }
        }

        public async Task<bool> UserExist(string userName)
        {
            //Query.
            if (await _db.TblUsuario.AnyAsync(u => u.NombreUsuario.ToLower().Equals(userName.ToLower())))
            {
                return true;
            }  
            return false; 
        }

        private void CreatPassHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Query
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hmac.Key;
                passwordSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        

        private string CreatToken(Usuario user)
        {
            //Query.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.NombreUsuario)
            };

            //Query.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            //Query.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Query.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddMonths(500),
                SigningCredentials = creds
            };

            //query.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}