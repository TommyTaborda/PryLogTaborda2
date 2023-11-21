using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryLogTaborda
{
    public partial class frmPrincipal : Form
    {
        // se declara el objeto de forma global//
        clsAccesoBD objAccesoBD = new clsAccesoBD();
        public frmPrincipal()
        {
            InitializeComponent();


        }

        private void btnConectar_Click(object sender, EventArgs e)
        {

            objAccesoBD.ConectarBaseDatos();


            lblEstadoConexion.Text = objAccesoBD.EstadoConexion;

            //MessageBox.Show(objAccesoBD.EstadoConexion);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            objAccesoBD.TraerDatosDataSet(dgvRegistros);

            if (txtID.Text != "")
            {
                objAccesoBD.RegistrarDatosDataSet(txtID.Text);
            }
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            // trae los datos en la grilla//
            objAccesoBD.TraerDatos(dgvRegistros);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            objAccesoBD.TraerDatosDataSet(dgvRegistros);

            //si la caja de texto es distinta de ID, entonces lo ingreso//
            if (txtID.Text != "")
            {
                objAccesoBD.RegistrarDatosDataSet(txtID.Text);
            }


        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void lblEstadoConexion_Click(object sender, EventArgs e)
        {

        }
    }
}