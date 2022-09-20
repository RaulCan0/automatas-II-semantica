//Alumno Raúl Cano Briseño
//Requerimiento 1: Eliminar las comillas del printf e interpretar 
//                las secuencias de escape dentro de la cadena.
//Requerimiento 2: Marcar los errores sintácticos cuando la variable no exista.
//Requerimiento 3: Modificar el valor de la variable en la Asignacion.
//Requerimiento 4: Obtener el valor de la variable cuando se requiera y programar el método getValor()
//Requerimiento 5: Modificar el valor de la variable en el Scanf.
using System.Collections.Generic;

namespace Evalua
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> listaVariables = new List<Variable>();
        Stack<float> stackOperandos = new Stack<float>();
        public Lenguaje()
        {

        }
        public Lenguaje(string nombre) : base(nombre)
        {

        }
        private void addVariable(string name, Variable.TipoDato type)
        {
            listaVariables.Add(new Variable(name, type));
        }
        private void displayVariables()
        {
            Log.WriteLine("\nVariables: ");
            foreach (Variable v in listaVariables)
            {
                Log.WriteLine(v.getNombre() + " " + v.getTipo() + " " + v.getValue());
            }
        }
        private bool existeVariable(string name)
        {
            foreach (Variable v in listaVariables)
            {
                if (v.getNombre().Equals(name))
                    return true;
            }
            return false;
        }
        private void modValor(string name, float newValue)
        {
            //Requerimiento 3
            //Requerimiento 3: Modificar el valor de la variable en la Asignacion.
            foreach (Variable v in listaVariables)
            {
             if(v .getNombre().Equals(name))
             {
                 v.setValor(newValue);
             }
            }

        }
        private float getValor(string nameVariable)
        {
           //Requerimiento 4
           //Requerimiento 4: Obtener el valor de la variable cuando se requiera y programar el método getValor()
           foreach (Variable v in listaVariables)

           {
            if (v.getNombre().Equals(nameVariable))
             {
                return v.getValue();
             }
            
           }
           return 0;
           
        }
        //Programa -> Librerias? Variables? Main
        public void Programa()
        {
            Librerias();
            Variables();
            Main();
            displayVariables();
        }
        // Librerias -> #include<identificador(.h)?> Librerias?
        private void Librerias()
        {
            if (getContenido() == "#")
            {
                match("#");
                match("include");
                match("<");
                match(tipos.Identificador);
                if (getContenido() == ".")
                {
                    match(".");
                    match("h");
                }
                match(">");
                Librerias();
            }
        }
        // Variables -> tipoDato Lista_identificadores ; Variables?
        private void Variables()
        {
            if (getClasificacion() == tipos.TipoDato)
            {
                Variable.TipoDato type = Variable.TipoDato.Char;
                switch (getContenido())
                {
                    case "int": type = Variable.TipoDato.Int; break;
                    case "float": type = Variable.TipoDato.Float; break;
                }
                match(tipos.TipoDato);
                Lista_identificadores(type);
                match(tipos.FinSentencia);
                Variables();
            }
        }
        // Lista_identificadores -> identificador (,Lista_identificadores)?
        private void Lista_identificadores(Variable.TipoDato type)
        {
            if (getClasificacion() == Token.tipos.Identificador)
            {
                if (!existeVariable(getContenido()))
                    addVariable(getContenido(), type);
                else
                    throw new Error("Error de sintáxis. La variable duplicada \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
            match(tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(type);
            }
        }
        // Bloque_Instrucciones -> {Lista_Instrucciones?}
        private void Bloque_Instrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                Lista_Instrucciones();
            }
            match("}");
        }
        // Lista_Instrucciones -> Instruccion Lista_Instrucciones?
        private void Lista_Instrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                Lista_Instrucciones();
            }
        }
        // Instruccion -> Printf | Scanf | If | While | Do | For | Switch | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "if")
                If();
            else if (getContenido() == "do")
                Do();
            else if (getContenido() == "while")
                While();
            else if (getContenido() == "for")
                For();
            else if (getContenido() == "switch")
                Switch();
            else if (getContenido() == "printf")
                Printf();
            else if (getContenido() == "scanf")
                Scanf();
            else
            {
                Asignacion();
                //Console.WriteLine("Error de sintaxis. No se reconoce la instruccion: " + getContenido());
                //nextToken();
            }
        }
        // Asignacion -> identificador = cadena | Expresion ;
        private void Asignacion()
        {
            //Requerimiento 2. Si no existe la variable, se levanta la excepción.
            if (!existeVariable(getContenido()))
            {
                throw new Error("Error de sintáxis: La variable no existe \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
            Log.WriteLine();
            Log.Write(getContenido() + " = ");
            string name = getContenido();
            match(tipos.Identificador);
            match("=");
            Expresion();
            match(";");
            float resultado = stackOperandos.Pop();
            Log.Write("= " + resultado);
            Log.WriteLine();
            modValor(name, resultado);
        }
        // Printf -> printf (string | Expresion);
        private void Printf()
        {
            //Requerimiento 1: Eliminar las comillas del printf e interpretar 
           //                las secuencias de escape dentro de la cadena.
            match("printf");
            match("(");
            if (getClasificacion() == tipos.Cadena)
            {
                string comillas = getContenido();
                comillas = comillas.Replace("\\n" , "\n");
                comillas = comillas.Replace("\\t" , "\t");
                Console.Write(comillas.Substring(1, comillas.Length - 2));
                match(tipos.Cadena);
            }
            else
            {
                Expresion();
                Console.Write(stackOperandos.Pop());
            }
            match(")");
            match(";");
        }
        // Scanf -> scanf (string, &Identificador);
        private void Scanf() 
        {
            match("scanf");
            match("(");
            match(tipos.Cadena);
            match(",");
            match("&");
            //Requerimiento 2. Si no existe la variable, se levanta la excepción.
            if (!existeVariable(getContenido()))
            {
                throw new Error("Error de sintáxis: Variable no existe \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
            string value = "" + Console.ReadLine();
            float valor = float.Parse(value);
            //Requerimiento 5. Modificar el valor de la variable.
            modValor(getContenido(), valor);
            match(tipos.Identificador);
            match(")");
            match(";");
        }
        // If -> if (Condicion) Bloque_Instrucciones (else Bloque_Instrucciones)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
                Bloque_Instrucciones();
            else
                Instruccion();
            if (getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                    Bloque_Instrucciones();
                else
                    Instruccion();
            }
        }
        // While -> while(Condicion) Bloque_Instrucciones | Instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
                Bloque_Instrucciones();
            else
                Instruccion();
        }
        // Do -> do Bloque_Instrucciones | Instruccion while(Condicion);
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
                Bloque_Instrucciones();
            else
                Instruccion();
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        // For -> for (Asignacion Condición ; Incremento) Bloque_Instrucciones | Instruccion
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            Condicion();
            match(";");
            Incremento();
            match(")");
            if (getContenido() == "{")
                Bloque_Instrucciones();
            else
                Instruccion();
        }
        // Incremento -> identificador ++ | --
        private void Incremento()
        {
            string variable = getContenido();
            //Requerimiento 2. Si no existe la variable, se levanta la excepción.
            if (!existeVariable(getContenido()))
            {
                throw new Error("Error de sintáxis: Variable no existe \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
            match(tipos.Identificador);
            if (getClasificacion() == tipos.IncrementoTermino)
            {
                if (getContenido()[0] == '+')
                {
                    match("++");
                    modValor(variable, getValor(variable) + 1);
                }
                else
                {
                    match("--");
                    modValor(variable, getValor(variable) - 1);
                }
            }
            else
                match(tipos.IncrementoTermino);
        }
        // Switch -> switch (Expresion) { Lista_Casos (default: (Lista_Instrucciones_Case | Bloque_Instrucciones)? (break;)? )? }
        private void Switch()
        {
            match("switch");
            match("(");
            Expresion();
            stackOperandos.Pop();
            match(")");
            match("{");
            Lista_Casos();
            if (getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() != "}" && getContenido() != "{")
                    Lista_Instrucciones_Case();
                else if (getContenido() == "{")
                    Bloque_Instrucciones();
                if (getContenido() == "break")
                {
                    match("break");
                    match(";");
                }
            }
            match("}");
        }
        // Lista_Casos -> case Expresion: (Lista_Instrucciones_Case | Bloque_Instrucciones)? (break;)? (Lista_Casos)?
        private void Lista_Casos()
        {
            if (getContenido() != "}" && getContenido() != "default")
            {
                match("case");
                Expresion();
                stackOperandos.Pop();
                match(":");
                if (getContenido() != "case" && getContenido() != "{")
                    Lista_Instrucciones_Case();
                else if (getContenido() == "{")
                    Bloque_Instrucciones();
                if (getContenido() == "break")
                {
                    match("break");
                    match(";");
                }
                Lista_Casos();
            }
        }
        // Lista_Instrucciones_Case -> Instruccion Lista_Instrucciones_Case?
        private void Lista_Instrucciones_Case()
        {
            Instruccion();
            if (getContenido() != "break" && getContenido() != "case" && getContenido() != "default" && getContenido() != "}")
                Lista_Instrucciones_Case();
        }
        // Condicion -> Expresion operadorRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(tipos.OperadorRelacional);
            Expresion();
            stackOperandos.Pop();
        }
        // Main -> void main() Bloque_Instrucciones 
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            Bloque_Instrucciones();
        }
        // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        // MasTermino -> (operadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(tipos.OperadorTermino);
                Termino();
                Log.Write(operador + " ");
                float n1 = stackOperandos.Pop();
                float n2 = stackOperandos.Pop();
                switch (operador)
                {
                    case "+":
                        stackOperandos.Push(n2 + n1);
                        break;
                    case "-":
                        stackOperandos.Push(n2 - n1);
                        break;
                }
            }
        }
        // Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        // PorFactor -> (operadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(tipos.OperadorFactor);
                Factor();
                Log.Write(operador + " ");
                float n1 = stackOperandos.Pop();
                float n2 = stackOperandos.Pop();
                switch (operador)
                {
                    case "*":
                        stackOperandos.Push(n2 * n1);
                        break;
                    case "/":
                        stackOperandos.Push(n2 / n1);
                        break;
                }
            }
        }
        // Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == tipos.Numero)
            {
                Log.Write(getContenido() + " ");
                stackOperandos.Push(float.Parse(getContenido()));
                match(tipos.Numero);
            }
            else if (getClasificacion() == tipos.Identificador)
            {
                //Requerimiento 2. Si no existe la variable, se levanta la excepción.
                if (!existeVariable(getContenido()))
            {
                throw new Error("Error de sintáxis: Variable no existe \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
                Log.Write(getContenido() + " ");
                stackOperandos.Push(getValor(getContenido()));
                match(tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}