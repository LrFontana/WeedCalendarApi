using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core.Dtos.ResponseDto
{
    public class ResponsePaginatorDto
    {
        //Propiedades.
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int PageSize { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public object Resultado { get; set; }
        public string Mensaje { get; set; }
    }
}