using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            FileManager.Guardar("Prueba", "\".cmd prueba\"", true);
        }

        [TestMethod]
        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            string nombreCocinero = "CocineroPrueba";

            Cocinero<Hamburguesa> cocinero;
            cocinero = new Cocinero<Hamburguesa>(nombreCocinero);

            Assert.AreEqual(0, cocinero.CantPedidosFinalizados);
        }
    }
}