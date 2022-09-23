//Alumno Raúl Cano Briseño 
//Requerimiento 1: Actualizar el dominante para variables en la expresion.
//Requerimiento 2: Actualizar el dominante para el casteo
//Requerimiento 3: Modificar el valor de la variable en la Asignacion.
//Requerimiento 4: Obtener el valor de la variable cuando se requiera y programar el método getValor()
//Requerimiento 5: Modificar el valor de la variable en el Scanf.
using System.Collections.Generic;

namespace SEMANTICA
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> listaVariables = new List<Variable>();
        Stack<float> stackOperandos = new Stack<float>();
        Variable.TipoDato dominante;
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
            foreach (Variable v in listaVariables)
            {
                if (v.getNombre().Equals(name))
                {
                    v.setValor(newValue);
                }
            }
        }
        private float getValor(string nameVariable)
        {
            //Requerimiento 4.
            foreach (Variable v in listaVariables)
            {
                if (v.getNombre().Equals(nameVariable))
                {
                    return v.getValue();
                }
            }
            return 0;
        }
        private Variable.TipoDato getTipo(string nameVariable)
        {
            foreach (Variable v in listaVariables)
            {
                if (v.getNombre().Equals(nameVariable))
                {
                    return v.getTipo();
                }
            }
            return Variable.TipoDato.Char;
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
                    throw new Error("Error de sintáxis. Variable duplicada \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
            match(tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(type);
            }
        }
        // Bloque_Instrucciones -> {Lista_Instrucciones?}
        private void Bloque_Instrucciones(bool evaluacion )
        {
            match("{");
            if (getContenido() != "}")
            {
                Lista_Instrucciones(evaluacion);
            }
            match("}");
        }
        // Lista_Instrucciones -> Instruccion Lista_Instrucciones?
        private void Lista_Instrucciones(bool evaluacion)
        {
            Instruccion();
            if (getContenido() != "}")
            {
                Lista_Instrucciones(evaluacion);
            }
        }
        // Instruccion -> Printf | Scanf | If | While | Do | For | Switch | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "printf")
                Printf();
            else if (getContenido() == "scanf")
                Scanf();
            else if (getContenido() == "if")
                If();
            else if (getContenido() == "while")
                While();
            else if (getContenido() == "do")
                Do();
            else if (getContenido() == "for")
                For();
            else if (getContenido() == "switch")
                Switch();
            else
            {
                Asignacion();
                //Console.WriteLine("Error de sintaxis. No se reconoce la instruccion: " + getContenido());
                //nextToken();
            }
        }
        private Variable.TipoDato evaluanumero(float resultado)
        {
            if(resultado % 1 != 0)
            {
                return Variable.TipoDato.Float;
            }
            if(resultado <= 255)
            {
                return Variable.TipoDato.Char;
            }
            else if(resultado <= 65535)
            {
                return Variable.TipoDato.Int;
            }
            return Variable.TipoDato.Float;
        }
        private bool evaluasemantica(string variable, float resultado)
        {
            Variable.TipoDato tipoDato = getTipo(variable);
            return false;
        }
        // Asignacion -> identificador = cadena | Expresion ;
        private void Asignacion()
        {
            //Requerimiento 2. Si no existe la variable, se levanta la excepción.
            if (!existeVariable(getContenido()))
            {
                throw new Error("Error de sintáxis: Variable no existe \"" + getContenido() + "\" en la línea " + linea + ".", Log);
            }
            Log.WriteLine();
            Log.Write(getContenido() + " = ");
            string name = getContenido();
            match(tipos.Identificador);
            match(tipos.Asignacion);
            dominante = Variable.TipoDato.Char;
            Expresion();
            match(";");
            float resultado = stackOperandos.Pop();
            Log.Write("= " + resultado);
            Log.WriteLine();
            if (dominante < evaluanumero(resultado))
            {
                dominante = evaluanumero(resultado);
            }
            if(dominante <= getTipo(name))
            {
                modValor(name, resultado);
            }
            else 
            {
                 throw new Error("Error de semantica: no podemos asignar un: <" +dominante + "> a un <" + getTipo(name) +  "> en linea  " + linea, Log);
            }
        }
        // Printf -> printf (string | Expresion);
        private void Printf()
        {
            match("printf");
            match("(");
            if (getClasificacion() == tipos.Cadena)
            {
                string comilla = getContenido();
                comilla = comilla.Replace("\\n" , "\n");
                comilla = comilla.Replace("\\t" , "\t");
                Console.Write(comilla.Substring(1, comilla.Length - 2));
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
            bool validarIf = Condicion();
            match(")");
            if (getContenido() == "{")
                Bloque_Instrucciones(validarIf);
            else
                Instruccion(else);
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
        private bool Condicion()
        {
            Expresion();
            String operador = getContenido();
            match(tipos.OperadorRelacional);
            Expresion();
            float e2 = stackOperandos.Pop();
            float e1 = stackOperandos.Pop();
            stackOperandos.Pop();
            switch (operador)
            {
                case "==":
                return e2 == e1;
                case ">":
                return e2 == e1;
                case ">=":
                return e2 == e1;
                case "<":
                return e2 == e1;
                case "<=":
                return e2 <= e1;
                default:
                return e1 != e2;
            }
            return false;
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
                if(dominante < evaluanumero(float.Parse(getContenido())))
                {
                    dominante = evaluanumero(float.Parse(getContenido()));
                }
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
                bool Hubocasteo = false;
                Variable.TipoDato casteo = Variable.TipoDato.Char;
                match("(");
                if (getClasificacion() == tipos.TipoDato)
                {
                     Hubocasteo = true;
                     match(tipos.TipoDato);
                     match("(");
                     match(")");
                }
                Expresion();
                match(")");
            }
        }
    }
}