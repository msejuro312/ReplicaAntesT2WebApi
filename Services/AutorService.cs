using Microsoft.Data.SqlClient;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AutorService: IAutorService
    {
        private readonly string? conexion;

        public AutorService(IConfiguration configuration) 
        {
            conexion = configuration.GetConnectionString("conexion");
        }

        public List<Autor> list()
        { 
            List<Autor> temporal = new List<Autor>();

            using (SqlConnection con = new SqlConnection(conexion))
            {
                using (SqlCommand command = new SqlCommand("sp_list_autores", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) 
                        {
                            Autor autor = new Autor
                            {
                                AutorId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                            };
                            temporal.
                                Add(autor);
                        }
                    }
                }
            }
            return temporal;
        }
    }
}
