using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Proyecto1Laboratorio_SosaFacundo
{
    public partial class consultarEliminar : Form
    {
        private Conexion cnn = new Conexion();
        public consultarEliminar()
        {
            InitializeComponent();

        }

        private void consultarEliminar_Load(object sender, EventArgs e)
        {
            List<Categoria> categorias = cnn.ObtenerCategorias();

            cmbCategoria.Items.Clear();
            foreach (Categoria cat in categorias)
            {
                cmbCategoria.Items.Add(cat);
            }
            cmbCategoria.SelectedIndexChanged += cmbCategoria_SelectedIndexChanged;
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            var categoriaSeleccionada = cmbCategoria.SelectedItem as Categoria;

            if (categoriaSeleccionada != null)
            {
                CargarProductosPorCategoria(categoriaSeleccionada.Id);
            }
        }

        private void CargarProductosPorCategoria(int idCategoria)
        {
            try
            {
                string query = "SELECT * FROM productos WHERE cod_categoria = @idCategoria";
                MySqlCommand cmd = new MySqlCommand(query, cnn.ObtenerConexion());
                cmd.Parameters.AddWithValue("@idCategoria", idCategoria);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);

                dgvListado.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

       

        private void EliminarProducto(int id)
        {
            try
            {
                string query = "DELETE FROM productos WHERE codigo = @id";
                MySqlCommand cmd = new MySqlCommand(query, cnn.ObtenerConexion());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Producto eliminado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar producto: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
          
            if (dgvListado.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccioná un producto para eliminar.");
                return;
            }

            int idProducto = Convert.ToInt32(dgvListado.SelectedRows[0].Cells["codigo"].Value);

            DialogResult resultado = MessageBox.Show("¿Estás seguro de que querés eliminar este producto?", "Confirmar eliminación", MessageBoxButtons.YesNo);

            if (resultado == DialogResult.Yes)
            {
                EliminarProducto(idProducto);

                var categoriaSeleccionada = cmbCategoria.SelectedItem as Categoria;
                if (categoriaSeleccionada != null)
                {
                    CargarProductosPorCategoria(categoriaSeleccionada.Id);
                }
            }
        }
    }
}
