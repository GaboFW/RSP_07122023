using Entidades.Exceptions;
using System.Text.Json;

namespace Entidades.Files
{
    public static class FileManager
    {
        private static string path;

        static FileManager()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            path = Path.Combine(path, "Prueba tp");

            ValidaExistenciaDeDirectorio();
        }

        private static void ValidaExistenciaDeDirectorio()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                    throw new FileManagerException("Error al crear el directorio");
                }
            }
            catch (FileManagerException ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
            }
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            string rutaArchivo = Path.Combine(path, nombreArchivo);

            try
            {
                if (append)
                {
                    File.AppendAllText(rutaArchivo, data);
                }
                else
                {
                    File.WriteAllText(rutaArchivo, data);
                }
            }
            catch (FileManagerException ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
            }
        }

        public static bool Serializar<T>(T elemento, string nombreArchivo) where T : class
        {
            try
            {
                string fullPath = Path.Combine(path, nombreArchivo);
                string jsonString = JsonSerializer.Serialize(elemento);
                File.WriteAllText(fullPath, jsonString);
            }
            catch (FileManagerException ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
            }

            return true;
        }
    }
}
