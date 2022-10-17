namespace Semantica
{
    public class Token
    {
        private string Contenido = "";
        private Tipos Clasificacion;
        public enum Tipos
        {
            Identificador,Numero,Caracter,Asignacion,Inicializacion,
            OperadorLogico,OperadorRelacional,OperadorTernario,
            OperadorTermino,OperadorFactor,IncrementoTermino,IncrementoFactor,
            FinSentencia,Cadena,TipoDato,Zona,Condicion,Ciclo
        }

        public void setContenido(string contenido)
        {
            this.Contenido = contenido;
        }

        public void setClasificacion(Tipos clasificacion)
        {
            this.Clasificacion = clasificacion;
        }

        public string getContenido()
        {
            return this.Contenido;
        }

        public Tipos getClasificacion()
        {
            return this.Clasificacion;
        }

    }
}