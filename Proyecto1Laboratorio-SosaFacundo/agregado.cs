using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Proyecto1Laboratorio_SosaFacundo
{
    public partial class agregado : Form
    {
        Conexion cnn = new Conexion();
        public agregado()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            consultarEliminar frmConsulta = new consultarEliminar();
            frmConsulta.Show();
        }

        private void agregado_Load(object sender, EventArgs e)
        {
            try
            {
                cnn.AbrirConexion();
                MessageBox.Show("Base de datos conectada correctamente");
                CargarCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            decimal precio = int.Parse(txtPrecio.Text);

            // Verificar que el precio sea un número válido
            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El precio debe ser un número válido.");
                return;
            }

            string categoriaSeleccionada = cmbCategoria.SelectedIndex.ToString() + 1;

            if (categoriaSeleccionada == null)
            {
                MessageBox.Show("Por favor, seleccione una categoría.");
                return;
            }

            try
            {
                // Crear una consulta SQL para insertar el nuevo producto
                string query = "INSERT INTO productos (nombre, descripcion, precio, cod_categoria) VALUES (@nombre, @descripcion, @precio, @cod_categoria)";

                MySqlCommand cmd = new MySqlCommand(query, cnn.ObtenerConexion());

                // Agregar parámetros para evitar SQL Injection
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@cod_categoria", categoriaSeleccionada);

                // Ejecutar la consulta
                int resultado = cmd.ExecuteNonQuery();

                if (resultado > 0)
                {
                    MessageBox.Show("Producto agregado correctamente.");
                }
                else
                {
                    MessageBox.Show("Hubo un error al agregar el producto.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }

        }

        private void CargarCategorias()
        {
            try
            {
                string query = "SELECT nombre FROM categorias";
                MySqlCommand cmd = new MySqlCommand(query, cnn.ObtenerConexion());
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbCategoria.Items.Add(reader.GetString("nombre"));
                }

                reader.Close(); // Muy importante cerrar el lector después
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
        }
    }
}
