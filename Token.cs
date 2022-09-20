namespace Evalua
{
    public class Token
    {
        private string Contenido;
        private tipos Clasificacion;
        public enum tipos
        {
            Identificador, Numero, Caracter, Asignacion, Inicializacion,
            OperadorLogico, OperadorRelacional, OperadorTernario,
            OperadorTermino, OperadorFactor, IncrementoTermino, IncrementoFactor,
            FinSentencia, Cadena,
            TipoDato, Zona, Condicion, Ciclo
        }
        public Token()   //Constructor
        {
            this.Contenido = "";
        }
        public void setContenido(string Contenido)
        {
            this.Contenido = Contenido;
        }
        public void setClasificacion(tipos Clasificacion)
        {
            this.Clasificacion = Clasificacion;
        }
        public string getContenido()
        {
            return this.Contenido;
        }
        public tipos getClasificacion()
        {
            return this.Clasificacion;
        }
    }
}