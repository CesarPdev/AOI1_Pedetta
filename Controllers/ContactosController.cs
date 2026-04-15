using AOI1_Pedetta.Models;
using AOI1_Pedetta.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AOI1_Pedetta.Controllers
{
    // Controlador de la API REST para gestionar Contactos.
    // Todos los endpoints requieren autenticación JWT.
    [ApiController]
    [Route("api/contacto")]
    [Authorize]                     // Protege todos los endpoints con JWT
    public class ContactosController : ControllerBase
    {
        private readonly ContactoService _service;

        public ContactosController(ContactoService service)
        {
            _service = service;
        }

        // GET api/contacto/{id}
        [HttpGet("{id}")]
        public ActionResult<Contacto> ObtenerPorId(int id)
        {
            var contacto = _service.ObtenerPorId(id);
            if (contacto is null)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return Ok(contacto);
        }

        // POST api/contacto/add
        [HttpPost("add")]
        public ActionResult<Contacto> Crear([FromBody] Contacto contacto)
        {
            var nuevo = _service.Crear(contacto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
        }

        // PUT api/contacto/edit/{id}
        [HttpPut("edit/{id}")]
        public ActionResult Editar(int id, [FromBody] Contacto contacto)
        {
            var editado = _service.Editar(id, contacto);
            if (!editado)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return NoContent();
        }
    }
}
