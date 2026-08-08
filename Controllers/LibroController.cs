using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ILibroService _service;

        public LibroController(ILibroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var libros = _service.list();
            return Ok(await Task.Run(() => libros));
        }

        [HttpGet("{LibroId}")]
        public async Task<IActionResult> GetById(int LibroId) 
        {
            var libro = _service.getById(LibroId);
            if (libro == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    message = "No se encontró el libro con el id " + LibroId,
                    success = false,
                    data = ""
                });
            }
            else
            {
                return Ok(new ApiResponse<Libro>
                {
                    message = "Libro encontrado!",
                    success = true,
                    data = libro
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Libro libro)
        {
            bool resp = _service.insert(libro);
            if (resp)
            {
                return Ok(new ApiResponse<Libro>
                {
                    message = "Libro insertado correctamente!",
                    success = true,
                    data = libro
                });
            }
            else
            {
                return BadRequest(new ApiResponse<object>
                {
                    message = "No se registró libro!",
                    success = false,
                    data = ""
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Libro libro)
        {
            Libro lib = _service.getById(libro.LibroId);
            if (lib == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    message = "No se encontró libro para actualizar!",
                    success = false,
                    data = ""
                });
            }
            else
            {
                bool resp = _service.update(libro);
                if (resp)
                {
                    return Ok(new ApiResponse<Libro>
                    {
                        message = "Libro actualizado correctamente!",
                        success = true,
                        data = libro
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        message = "Ocurrió un error al actualizar libro!",
                        success = false,
                        data = ""
                    });
                }

            }
        }

        [HttpDelete("{LibroId}")]
        public async Task<IActionResult> Delete(int LibroId)
        {
            Libro lib = _service.getById(LibroId);
            if (lib == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    message = "No se encontró libro para eliminar!",
                    success = false,
                    data = ""
                });
            }
            else
            {
                bool resp = _service.delete(LibroId);
                if (resp)
                {
                    return Ok(new ApiResponse<Libro>
                    {
                        message = "Libro eliminado correctamente!",
                        success = true,
                        data = lib
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        message = "Ocurrió un error al eliminar libro!",
                        success = false,
                        data = ""
                    });
                }
                
            }
        }


    }
}
