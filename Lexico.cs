namespace Evalua
{
    public class Lexico : Token
    {
        StreamReader archivo;
        protected StreamWriter Log;
        const int F = -1;
        const int E = -2;
        protected int linea;
        int[,] TRAND =new int[,]
        {   
            //WS,EF,EL,L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, /, ", ', #, ?,La
            { 0, F, 0, 1, 2,40, 1,21,22, 8,10,12,13,14,15,18,19,24,24,29,26,33,36,28,40}, //estado 0
            { F, F, F, 1, 1, F, 1, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 1
            { F, F, F, F, 2, 3, 5, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 2
            { E, E, E, E, 4, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E}, //estado 3
            { F, F, F, F, 4, F, 5, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 4
            { E, E, E, E, 7, E, E, 6, 6, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E}, //estado 5
            { E, E, E, E, 7, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E}, //estado 6
            { F, F, F, F, 7, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 7
            { F, F, F, F, F, F, F, F, F, 9, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 8
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 9
            { F, F, F, F, F, F, F, F, F,11, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 10
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 11
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 12
            { F, F, F, F, F, F, F, F, F, F, F, F,16, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 13
            { F, F, F, F, F, F, F, F, F, F, F, F, F,16, F, F, F, F, F, F, F, F, F, F, F}, //estado 14
            { F, F, F, F, F, F, F, F, F,17, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 15
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 16
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 17
            { F, F, F, F, F, F, F, F, F,20, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 18
            { F, F, F, F, F, F, F, F, F,20, F, F, F, F, F,20, F, F, F, F, F, F, F, F, F}, //estado 19
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 20
            { F, F, F, F, F, F, F,23, F,23, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 21
            { F, F, F, F, F, F, F, F,23,23, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 22
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 23
            { F, F, F, F, F, F, F, F, F,25, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 24
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 25
            {26, E, E,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,26,27,26,26,26,26}, //estado 26
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 27
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 28
            { F, F, F, F, F, F, F, F, F,25, F, F, F, F, F, F, F,31, F,30, F, F, F, F, F}, //estado 29
            {30, 0, 0,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30}, //estado 30
            {31, E,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,32,31,31,31,31,31,31,31}, //estado 31
            {31, E,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,32,31, 0,31,31,31,31,31}, //estado 32
            {34, E, E,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34,34}, //estado 33
            { E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E, E,35, E, E, E}, //estado 34
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 35
            { F, F, F, F,37, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 36
            { F, F, F, F,38, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 37
            { F, F, F, F,39, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 38
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 39
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //estado 40
            //WS,EF,EL,L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, /, ", ', #, ?,La
        };
        public Lexico()   //Constructor 1
        {
            linea = 1;
            Log = new StreamWriter("C:\\EVALUAA\\Prueba.log");
            Log.AutoFlush = true;
            //Log.WriteLine("Primer constructor");
            Log.WriteLine("Archivo: prueba.cpp");
            Log.WriteLine("Compilado: " + DateTime.Now.ToString("El dd/MM/yyyy a la\'s\' HH:mm:ss")); //Requerimiento 1.
            //Investigar como checar si existe un archivo o no.
            if (File.Exists(@"C:\EVALUAA\Prueba.cpp"))
            {
                archivo = new StreamReader("C:\\EVALUAA\\Prueba.cpp");
            }
            else
            {
                throw new Error("Error: El archivo prueba no existe.", Log);
            }
        }
        public Lexico(string nombre)   //Constructor 2
        {
            linea = 1;
            Log = new StreamWriter(Path.ChangeExtension(nombre, ".log"));
            //Usar el objeto Path
            //Path.HasExtension(path1);
            Log.AutoFlush = true;
            //Log.WriteLine("Segundo constructor");
            Log.WriteLine("Archivo: " + nombre);
            Log.WriteLine("Compilado: " + DateTime.Now.ToString("El dd/MM/yyyy a la\'s\' HH:mm:ss")); //Requerimiento 1.
            //Investigar como checar si existe un archivo o no.
            if (File.Exists(nombre))
            {
                archivo = new StreamReader(nombre);
            }
            else
            {
                throw new Error("Error: El archivo con ruta -> " + nombre + " no existe.", Log);
            }
        }
        public void close()
        {
            archivo.Close();
            Log.Close();
        }
        private void clasifica(int estado)
        {
            switch (estado)
            {
                case 1:
                    setClasificacion(tipos.Identificador);
                    break;
                case 2:
                    setClasificacion(tipos.Numero);
                    break;
                case 8:
                    setClasificacion(tipos.Asignacion);
                    break;
                case 9:
                case 17:
                case 18:
                case 19:
                    setClasificacion(tipos.OperadorRelacional);
                    break;
                case 10:
                case 13:
                case 14:
                case 33:
                case 36:
                case 40:
                    setClasificacion(tipos.Caracter);
                    break;
                case 11:
                    setClasificacion(tipos.Inicializacion);
                    break;
                case 12:
                    setClasificacion(tipos.FinSentencia);
                    break;
                case 15:
                case 16:
                    setClasificacion(tipos.OperadorLogico);
                    break;
                case 21:
                case 22:
                    setClasificacion(tipos.OperadorTermino);
                    break;
                case 23:
                    setClasificacion(tipos.IncrementoTermino);
                    break;
                case 24:
                case 29:
                    setClasificacion(tipos.OperadorFactor);
                    break;
                case 25:
                    setClasificacion(tipos.IncrementoFactor);
                    break;
                case 26:
                    setClasificacion(tipos.Cadena);
                    break;
                case 28:
                    setClasificacion(tipos.OperadorTernario);
                    break;
            }
        }
        private int columna(char c)
        {
            //WS,EF,EL,L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, /, ", ', #, ?,La
            if (findArchivo())
            {
                return 1;
            }
            else if (c == '\n')
            {
                return 2;
            }
            else if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.ToUpper(c) == 'E')
            {
                return 6;
            }
            else if (char.IsLetter(c))
            {
                return 3;
            }
            else if (char.IsDigit(c))
            {
                return 4;
            }
            else if (c == '.')
            {
                return 5;
            }
            else if (c == '+')
            {
                return 7;
            }
            else if (c == '-')
            {
                return 8;
            }
            else if (c == '=')
            {
                return 9;
            }
            else if (c == ':')
            {
                return 10;
            }
            else if (c == ';')
            {
                return 11;
            }
            else if (c == '&')
            {
                return 12;
            }
            else if (c == '|')
            {
                return 13;
            }
            else if (c == '!')
            {
                return 14;
            }
            else if (c == '>')
            {
                return 15;
            }
            else if (c == '<')
            {
                return 16;
            }
            else if (c == '*')
            {
                return 17;
            }
            else if (c == '%')
            {
                return 18;
            }
            else if (c == '/')
            {
                return 19;
            }
            else if (c == '"')
            {
                return 20;
            }
            else if (c == '\'')
            {
                return 21;
            }
            else if (c == '#')
            {
                return 22;
            }
            else if (c == '?')
            {
                return 23;
            }
            return 24;
            //WS,EF,EL,L, D, ., E, +, -, =, :, ;, &, |, !, >, <, *, %, /, ", ', #, ?,La
        }
        public void nextToken()
        {
            char c;
            string buffer = "";
            int estado = 0;
            while (estado >= 0)
            {
                c = (char)archivo.Peek();  //Funcion de transición del autómata.
                estado = TRAND[estado, columna(c)];
                clasifica(estado);
                if (estado >= 0)
                {
                    archivo.Read();
                    if (c == '\n')
                    {
                        linea++;
                    }
                    if (estado > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer = "";
                    }
                }
            }
            setContenido(buffer);
            switch (buffer)
            {
                case "char":
                case "int":
                case "float":
                    setClasificacion(tipos.TipoDato);
                    break;
                case "private":
                case "public":
                case "protected":
                    setClasificacion(tipos.Zona);
                    break;
                case "if":
                case "else":
                case "switch":
                    setClasificacion(tipos.Condicion);
                    break;
                case "while":
                case "for":
                case "do":
                    setClasificacion(tipos.Ciclo);
                    break;
            }
            if (estado == E)
            {
                //Requerimiento 9: Agregar el número de linea en el error.
                if (getContenido()[0] == '"')
                {
                    throw new Error("Error léxico en linea " + linea + ". " + "No se cerro el string con: \"",Log);
                }
                else if (getContenido()[0] == '\'')
                {
                    throw new Error("Error léxico en linea " + linea + ". " + "No se cerro el caracter con: '",Log);
                }
                else if (getClasificacion() == tipos.Numero)
                {
                    throw new Error("Error léxico en linea " + linea + ". " + "Se espera un dígito.",Log);
                }
                else
                {
                    throw new Error("Error léxico desconocido en linea " + linea + ". ",Log);
                }
            }
            else if (!findArchivo())
            {
                //Log.WriteLine(getContenido() + " | " + getClasificacion());
            }
        }
        public bool findArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}