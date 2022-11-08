// Raul Cano Briseño
using System;
using System.IO;

namespace Semantica
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using(Lenguaje a = new Lenguaje())
                {
                    a.Programa();
                    //a.cerrar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}