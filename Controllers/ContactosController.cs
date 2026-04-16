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
    [Authorize] // Protege todos los endpoints con JWT
    public class ContactosController(ContactoService service) : ControllerBase
    {
        // GET api/contacto/{id}
        [HttpGet("{id}")]
        public ActionResult<Contacto> ObtenerPorId(int id)
        {
            var contacto = service.ObtenerPorId(id);
            if (contacto is null)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return Ok(contacto);
        }

        // POST api/contacto/add
        [HttpPost("add")]
        public ActionResult<Contacto> Crear([FromBody] Contacto contacto)
        {
            var nuevo = service.Crear(contacto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
        }

        // PUT api/contacto/edit/{id}
        [HttpPut("edit/{id}")]
        public ActionResult Editar(int id, [FromBody] Contacto contacto)
        {
            var editado = service.Editar(id, contacto);
            if (!editado)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return NoContent();
        }

        // PATCH api/contacto/edit/telefono/{id}
        [HttpPatch("edit/telefono/{id}")]
        public ActionResult ActualizarTelefono(int id, [FromBody] string telefono)
        {
            var editado = service.ActualizarTelefono(id, telefono);
            if (!editado)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return NoContent();
        }

        // PATCH api/contacto/edit/email/{id}
        [HttpPatch("edit/email/{id}")]
        public ActionResult ActualizarEmail(int id, [FromBody] string email)
        {
            var editado = service.ActualizarEmail(id, email);
            if (!editado)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return NoContent();
        }

        // DELETE api/contacto/delete/{id}
        [HttpDelete("delete/{id}")]
        public ActionResult Eliminar(int id)
        {
            var eliminado = service.Eliminar(id);
            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró el contacto con Id {id}." });

            return NoContent();
        }
    }
}
