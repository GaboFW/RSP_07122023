using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection = "Server = DESKTOP-GKRBQF4; Database = 20230622SP;Trusted_Connection = True;";

        static DataBaseManager()
        {
            connection = new SqlConnection(stringConnection);
        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                string consulta = "SELECT * FROM comidas WHERE tipo_comida = @Tipo";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@Tipo", tipo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            return reader["imagen"].ToString();
                        }
                        else
                        {
                            throw new ComidaInvalidaExeption("No existe el tipo de comida");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);

                throw new ComidaInvalidaExeption("Error al encontrar la imagen");
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            bool result = false;
            
            try
            {
                string consulta = "INSERT INTO Tickets (Empleado, Ticket) VALUES (@Empleado, @Ticket)";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@Empleado", nombreEmpleado);
                    command.Parameters.AddWithValue("@Ticket", comida.Ticket);
                    connection.Open();

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);

                throw new DataBaseManagerException("No se pudo guardar el ticket: ", ex);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }
    }
}
