using Microsoft.Data.SqlClient;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class LibroService: ILibroService
    {
        private readonly string? conexion;

        public LibroService(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("conexion");
        }

        public List<Libro> list()
        {
            List<Libro> temporal = new List<Libro>();

            using (SqlConnection con = new SqlConnection(conexion))
            {
                using (SqlCommand command = new SqlCommand("sp_list_libros", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Libro libro = new Libro
                            {
                                LibroId = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                ISBN = reader.GetString(2),
                                AnioPublicacion = reader.GetInt32(3),
                                Autor = reader.GetString(4),
                            };
                            temporal.
                                Add(libro);
                        }
                    }
                }
            }
            return temporal;
        }
        
        public Libro getById(int LibroId)
        {
            Libro libro = null;

            using (SqlConnection con = new SqlConnection(conexion))
            {
                using (SqlCommand command = new SqlCommand("sp_find_libro_by_id", con))
                {
                    
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LibroId", LibroId);
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            libro = new Libro
                            {
                                LibroId = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                ISBN = reader.GetString(2),
                                AnioPublicacion = reader.GetInt32(3),
                                Autor = reader.GetString(4),
                            };
                            
                        }
                    }
                }
            }
            return libro;
        }

        public bool insert(Libro libro)
        {
            bool resp = false;

            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    using (SqlCommand command = new SqlCommand("sp_insert_libro", con))
                    { 
                        command.Transaction = tran;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AutorId", libro.AutorId);
                        command.Parameters.AddWithValue("@Titulo", libro.Titulo);
                        command.Parameters.AddWithValue("@ISBN", libro.ISBN);
                        command.Parameters.AddWithValue("@AnioPublicacion", libro.AnioPublicacion);
                        resp = command.ExecuteNonQuery()>0;
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }                
                
            }
            return resp;
        }

        
        public bool update(Libro libro)
        {
            bool resp = false;

            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    using (SqlCommand command = new SqlCommand("sp_update_libro", con))
                    {
                        command.Transaction = tran;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LibroId", libro.LibroId);
                        command.Parameters.AddWithValue("@AutorId", libro.AutorId);
                        command.Parameters.AddWithValue("@Titulo", libro.Titulo);
                        command.Parameters.AddWithValue("@ISBN", libro.ISBN);
                        command.Parameters.AddWithValue("@AnioPublicacion", libro.AnioPublicacion);
                        resp = command.ExecuteNonQuery() > 0;
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }

            }
            return resp;
        }

        public bool delete(int LibroId)
        {
            bool resp = false;

            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    using (SqlCommand command = new SqlCommand("sp_delete_libro", con))
                    {
                        command.Transaction = tran;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LibroId",LibroId);
                        
                        resp = command.ExecuteNonQuery() > 0;
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }

            }
            return resp;
        }
    }
}
