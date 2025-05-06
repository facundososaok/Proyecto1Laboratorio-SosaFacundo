using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Proyecto1Laboratorio_SosaFacundo
{
    public class Conexion
    {
        private MySqlConnection conexion;
        private string cadenaConexion = "server=localhost;port=3306;user=root;password=root;database=productos";

        public MySqlConnection AbrirConexion()
        {
            if (conexion == null)
                conexion = new MySqlConnection(cadenaConexion);

            if (conexion.State == System.Data.ConnectionState.Closed)
                conexion.Open();

            return conexion;
        }

        public void CerrarConexion()
        {
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();
        }
        public MySqlConnection ObtenerConexion()
        {
            return conexion;
        }
    }
}
