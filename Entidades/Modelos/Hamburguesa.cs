using Entidades.DataBase;
using Entidades.Enumerados;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {
        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;

        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        public Hamburguesa() : this(false) { }

        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        public bool Estado { get { return this.estado; } }

        public string Imagen { get { return this.imagen; } }

        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();
        }

        public override string ToString() => this.MostrarDatos();

        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = ingredientes.CalcularCostolngrediente(costoBase);

            this.estado = true;
        }

        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int numeroRandom = this.random.Next(1, 9);

                this.imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{numeroRandom}");

                this.AgregarIngredientes();
            }
        }
    }
}