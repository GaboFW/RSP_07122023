using Entidades.DataBase;
using Entidades.Interfaces;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoPedido<T>(T menu); //VER QUE ONDA

    public class Mozo<T> where T : IComestible, new()
    {
        private CancellationTokenSource cancellation;

        private T menu;

        private Task tarea;

        public event DelegadoNuevoPedido<T> OnPedido;

        public bool EmpezarATrabajar
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running || this.tarea.Status == TaskStatus.WaitingToRun || this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && this.tarea is null || this.tarea.Status != TaskStatus.Running || this.tarea.Status != TaskStatus.WaitingToRun || this.tarea.Status != TaskStatus.WaitingForActivation)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.TomarPedidos();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        private void NotificarNuevoPedido()
        {
            if (this.OnPedido is not null)
            {
                this.menu = new T();

                this.menu.IniciarPreparacion();

                this.OnPedido.Invoke(this.menu);
            }
        }

        private void TomarPedidos()
        {
            this.tarea = Task.Run(() =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    NotificarNuevoPedido();

                    Thread.Sleep(5000);

                }
            }, cancellation.Token);
        }
    }
}
