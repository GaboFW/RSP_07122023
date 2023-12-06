using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostolngrediente(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoTotal = costoInicial;

            foreach (EIngrediente ingrediente in ingredientes)
            {
                costoTotal += costoInicial * (double)ingrediente / 100;
            }

            return costoTotal;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON,
            };

            int cantidadIngredientes = rand.Next(1, ingredientes.Count + 1);

            return ingredientes.Take(cantidadIngredientes).ToList();
        }
    }
}
