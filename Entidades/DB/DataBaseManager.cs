using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
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
            string resultado = null;

            try
            {
                string consulta = "SELECT * FROM Comidas WHERE Tipo = @Tipo";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@Tipo", tipo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            resultado = reader.GetString(1);
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
                throw new DataBaseManagerException("Error al encontrar el tipo: ", ex);
            }

            return resultado;
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible
        {
            bool result = false;
            
            try
            {
                string consulta = "INSERT INTO Tickets (NombreEmpleado, Comida) VALUES (@NombreEmpleado, @Comida)";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@NombreEmpleado", nombreEmpleado);
                    command.Parameters.AddWithValue("@Comida", comida.Ticket);
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
                throw new DataBaseManagerException("No se pudo guardar el ticket por: ", ex);
            }

            return result;
        }
    }
}
