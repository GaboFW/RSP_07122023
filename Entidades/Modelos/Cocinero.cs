using Entidades.DataBase;
using Entidades.Interfaces;

namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoPedidoEnCurso(IComestible menu);

    public class Cocinero<T> where T : IComestible, new()
    {
        // 07-12-2023
        private Mozo<T> mozo;
        private Queue<T> pedidos;
        //

        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidoEnPreparacion;

        private Task tarea;

        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoPedidoEnCurso OnPedido;

        public Cocinero(string nombre)
        {
            this.nombre = nombre;

            // 07-12-2023
            this.mozo = new Mozo<T>();
            this.Pedidos = new Queue<T>();
            this.mozo.OnPedido += TomarNuevoPedido; 
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

                    this.mozo.EmpezarATrabajar = value;

                    this.EmpezarACocinar();
                }
                else
                {
                    this.mozo.EmpezarATrabajar = !value;

                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }

        public string Nombre { get => nombre; }

        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        // 07-12-2023
        public Queue<T> Pedidos { get; }
        //

        private void EmpezarACocinar()
        {
            this.tarea = Task.Run(() =>
            { 
                while (!cancellation.IsCancellationRequested)
                {
                    if (this.pedidos.Count > 0 && this.OnPedido is not null)
                    {
                        this.pedidoEnPreparacion = this.pedidos.Dequeue();
                        EsperarProximoIngreso();
                        this.cantPedidosFinalizados++;
                        DataBaseManager.GuardarTicket<T>(this.nombre, this.pedidoEnPreparacion);
                    }
                }
            }, cancellation.Token);
        }

        private void EsperarProximoIngreso()
        {
            if (this.OnDemora is not null)
            {
                int tiempoEspera = 0;

                while (!this.pedidoEnPreparacion.Estado && !cancellation.IsCancellationRequested)
                {
                    this.OnDemora.Invoke(tiempoEspera);

                    Thread.Sleep(1000);

                    tiempoEspera += 1;
                }

                this.demoraPreparacionTotal += tiempoEspera;
            }
        }

        private void TomarNuevoPedido(T menu)
        {
            if (this.OnPedido is not null)
            {
                this.pedidos.Enqueue(menu);
            }
        }
    }
}
