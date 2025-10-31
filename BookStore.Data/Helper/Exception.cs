using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Helper
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class AuthorizedException : Exception
    {
        public AuthorizedException(string message) : base(message)
        {
        }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }

    public class FileException : Exception
    {
        public FileException(string message) : base(message)
        {

        }
    }
    
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {

        }
    }
}
