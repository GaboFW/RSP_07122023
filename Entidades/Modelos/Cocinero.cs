using Entidades.DataBase;
using Entidades.Interfaces;
using System.Net.Http.Headers;

namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoIngreso(IComestible menu);

    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T menu;

        private Task tarea;

        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoNuevoIngreso OnIngreso;

        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }

        public string Nombre { get => nombre; }

        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        private void IniciarIngreso()
        {
            while (this.HabilitarCocina)
            {
                this.tarea = Task.Run(NotificarNuevoIngreso);

                this.cantPedidosFinalizados++;

                DataBaseManager.GuardarTicket(this.nombre, this.menu);

                Task.Delay(3000);
            }
        }

        private void NotificarNuevoIngreso()
        {
            while (true)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();

                if (this.OnIngreso is not null)
                {
                    this.EsperarProximoIngreso();
                }
                else
                {
                    this.OnIngreso.Invoke(menu);
                }
            }
        }

        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;
            if (this.OnDemora is not null)
            {
                this.demoraPreparacionTotal += tiempoEspera;
            }
            else
            {
                this.OnDemora.Invoke(this.demoraPreparacionTotal);
            }
        }
    }
}
