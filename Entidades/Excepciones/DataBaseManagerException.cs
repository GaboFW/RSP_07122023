namespace Entidades.Exceptions
{
    public class DataBaseManagerException : Exception
    {
        public DataBaseManagerException(string message)
        {

        }

        public DataBaseManagerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}
