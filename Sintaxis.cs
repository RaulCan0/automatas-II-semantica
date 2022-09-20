using System;

namespace Evalua
{
    public class Sintaxis : Lexico
    {
        public Sintaxis()
        {
            nextToken();
        }
        public Sintaxis(string nombre) : base(nombre)
        {
            nextToken();
        }
        public void match(String espera)
        {
            if (espera == getContenido())
            {
                nextToken();
            }
            else
            {
                //Requerimiento 9: Agregar el número de linea en el error.
                throw new Error("Error de Sintaxis en linea " + linea + ". " + "Se espera " + espera, Log);
            }
        }
        public void match(tipos espera)
        {
            if (espera == getClasificacion())
            {
                nextToken();
            }
            else
            {
                //Requerimiento 9: Agregar el número de linea en el error.
                throw new Error("Error de Sintaxis en linea " + linea + ". " + "Se espera " + espera + ".", Log);
            }
        }
    }
}