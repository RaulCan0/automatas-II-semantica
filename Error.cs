using System;
using System.IO;

namespace Semantica
{
    public class Error : Exception
    {
        public Error(string mensaje, StreamWriter log) : base(mensaje)
        {
            log.WriteLine(mensaje); 
        }
    }
}