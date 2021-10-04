using System;
using System.Net;

namespace Core
{
    public class ExceptionHelper : Exception
    {
        public ExceptionHelper(string message, Exception innerException) : base(message, innerException) { }
        public ExceptionHelper(string message) : base(message) { }
    }
}
