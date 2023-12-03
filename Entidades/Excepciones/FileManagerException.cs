using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class FileManagerException : Exception
    {
        public FileManagerException() 
        {
            
        }

        public FileManagerException(string message)
        : base(message)
        {
            
        }

        public FileManagerException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
