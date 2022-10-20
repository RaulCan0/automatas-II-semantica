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
                Lenguaje a = new Lenguaje();

                a.Programa();

                /*while(!a.FinArchivo())
                {
                    a.NextToken();
                }*/

                a.cerrar();
               /* {
                    int b = 3;
                    b = 4;
                }
                b = 5;*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}