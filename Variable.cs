namespace Evalua
{
    public class Variable
    {
        public enum TipoDato
        {
            Char, Int, Float
        }
        string name;
        float value;
        TipoDato type;

        public Variable(string name, TipoDato type)
        {
            this.name = name;
            this.type = type;
            this.value = 0;
        }
        public void setValor(float value)
        {
            this.value = value;
        }
        public float getValue()
        {
            return this.value;
        }
        public string getNombre()
        {
            return this.name;
        }
        public TipoDato getTipo()
        {
            return this.type;
        }
    }
}