using Entidades.Files;
using Entidades.Interfaces;
using Entidades.Modelos;
using System.Security.Cryptography.X509Certificates;


namespace FrmView
{
    public partial class FrmView : Form
    {
        private Queue<IComestible> comidas;
        Cocinero<Hamburguesa> hamburguesero;

        private IComestible comida;

        public FrmView()
        {
            InitializeComponent();
            //this.comidas = new Queue<IComestible>();
            this.hamburguesero = new Cocinero<Hamburguesa>("Ramon");
            //Alumno - agregar manejadores al cocinero
            this.hamburguesero.OnDemora += this.MostrarConteo;
            this.hamburguesero.OnPedido += this.MostrarComida;
        }

        //Alumno: Realizar los cambios necesarios sobre MostrarComida de manera que se refleje
        //en el formulario los datos de la comida
        private void MostrarComida(IComestible comida)
        {
            if (!this.InvokeRequired)
            {
                this.comidas.Enqueue(comida);
                this.pcbComida.Load(comida.Imagen);
                this.rchElaborando.Text = comida.ToString();
            }
            else
            {
                this.Invoke(new Action(() => MostrarComida(comida)));
            }
        }

        //Alumno: Realizar los cambios necesarios sobre MostrarConteo de manera que se refleje
        //en el fomrulario el tiempo transucurrido
        private void MostrarConteo(double tiempo)
        {
            if (!this.InvokeRequired)
            {
                this.lblTiempo.Text = $"{tiempo} segundos";
                this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion.ToString("00.0")} segundos";
            }
            else
            {
                this.Invoke(new Action(() => MostrarConteo(tiempo)));
            }
        }

        private void ActualizarAtendidos(IComestible comida)
        {
            this.rchFinalizados.Text += "\n" + comida.Ticket;
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                this.hamburguesero.HabilitarCocina = true;
                this.btnAbrir.Image = Properties.Resources.close_icon;
            }
            else
            {
                this.hamburguesero.HabilitarCocina = false;
                this.btnAbrir.Image = Properties.Resources.open_icon;
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (this.comidas is not null)
            {
                // IComestible comida = this.comidas.Dequeue();
                comida.FinalizarPreparacion(this.hamburguesero.Nombre);
                this.ActualizarAtendidos(comida);

                this.comida = null;
            }
            else
            {
                MessageBox.Show("El Cocinero no posee comidas", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void FrmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Alumno: Serializar el cocinero antes de cerrar el formulario

            FileManager.Serializar(this.hamburguesero, "Prueba tp.txt");
        }
    }
}