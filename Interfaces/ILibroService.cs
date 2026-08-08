
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ILibroService
    {
        List<Libro> list();
        Libro getById(int LibroId);
        bool insert(Libro libro);        
        bool update (Libro libro);
        bool delete (int LibroId);
       
    }
}
