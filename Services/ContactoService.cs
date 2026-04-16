using AOI1_Pedetta.Models;

namespace AOI1_Pedetta.Services
{
    // Servicio que gestiona una lista de Contactos en memoria.
    // Se registra como Singleton para que la data persista entre diferentes peticiones HTTP.
    public class ContactoService
    {
        // Lista en memoria que actúa como fuente de datos
        private readonly List<Contacto> _contactos =
        [
            new() { Id = 1, Nombre = "Juan",   Apellido = "Pérez",  Telefono = "2994001001", Email = "juan.perez@example.com"  },
            new() { Id = 2, Nombre = "María",  Apellido = "García", Telefono = "2994001002", Email = "maria.garcia@example.com" },
            new() { Id = 3, Nombre = "Carlos", Apellido = "López",  Telefono = "2994001003", Email = "carlos.lopez@example.com" }
        ];

        private int _nextId = 4; // Contador para auto-incremento de Id

        // Retorna todos los contactos.
        public List<Contacto> ObtenerTodos() => _contactos;

        // Retorna un contacto por su Id, o null si no existe.
        public Contacto? ObtenerPorId(int id) =>
            _contactos.FirstOrDefault(c => c.Id == id);

        // Agrega un nuevo contacto a la lista y le asigna un Id.
        public Contacto Crear(Contacto contacto)
        {
            contacto.Id = _nextId++;
            _contactos.Add(contacto);
            return contacto;
        }

        // Actualiza los datos de un contacto existente.
        // Retorna true si se encontró y editó, false si no existe.
        public bool Editar(int id, Contacto contacto)
        {
            var existente = ObtenerPorId(id);
            if (existente is null) return false;

            existente.Nombre = contacto.Nombre;
            existente.Apellido = contacto.Apellido;
            existente.Telefono = contacto.Telefono;
            existente.Email = contacto.Email;
            return true;
        }

        // Actualiza solo el teléfono de un contacto existente.
        // Retorna true si se encontró y editó, false si no existe.
        public bool ActualizarTelefono(int id, string telefono)
        {
            var existente = ObtenerPorId(id);
            if (existente is null) return false;

            existente.Telefono = telefono;
            return true;
        }

        // Actualiza solo el email de un contacto existente.
        // Retorna true si se encontró y editó, false si no existe.
        public bool ActualizarEmail(int id, string email)
        {
            var existente = ObtenerPorId(id);
            if (existente is null) return false;

            existente.Email = email;
            return true;
        }

        // Elimina un contacto por Id.
        // Retorna true si se encontró y eliminó, false si no existe.
        public bool Eliminar(int id)
        {
            var existente = ObtenerPorId(id);
            if (existente is null) return false;

            _contactos.Remove(existente);
            return true;
        }
    }
}
