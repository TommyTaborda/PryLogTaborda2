using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;// libreria que trae los genericos de acceso a datos/
using System.Windows.Forms;
using System.IO;

namespace pryLogTaborda
{
    internal class clsAccesoBD
    {
        public string EstadoConexion;
        public string Errores;
        public string DatosExtraidos; //string

        OleDbConnection conexionBD; // se crea objeto para hacer la conexion
        public string rutaArchivo;
        OleDbCommand comandoBD; //  se crea objeto que indica tareas para hacer dentro de la base de datos
        OleDbDataReader lectorBD;// lee de inicio a fin registros de datos

        OleDbDataAdapter adaptadorDS;// carga informacion a un DataSet////ejecuta lo que esta en el comnado, ej tae una tabla//
        DataSet objDataSet = new DataSet();// contiene varias bases de  datos o tablas de base de datos////graba, actualiza y borra// // se crea global para utilizar dentro del proyecto//
        public void ConectarBaseDatos()
        {
            try
            {
                if (rutaArchivo == null)
                {
                    rutaArchivo = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = ../../Base/Log.accdb";
                }
                else
                {
                    rutaArchivo = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + rutaArchivo;
                }


                conexionBD = new OleDbConnection();

                conexionBD.ConnectionString = rutaArchivo;

                conexionBD.Open();

                EstadoConexion = "Conectado a la base " + conexionBD.DataSource;
            }
            catch (Exception exepcion)
            {
                EstadoConexion = "Error en la conexión." + exepcion.Message;
            }

        }
        public void TraerDatos(DataGridView grilla)
        {
            //optimiza el código//
            try
            {
                //trae la tabla//
                comandoBD = new OleDbCommand();

                comandoBD.Connection = conexionBD;
                comandoBD.CommandType = System.Data.CommandType.TableDirect;
                comandoBD.CommandText = "Registros";

                //lo carga en el lector//
                lectorBD = comandoBD.ExecuteReader();

                //hago la grilla de forma dinámica, se  pone nombre de la grilla y el nombre del código//
                grilla.Columns.Add("ID", "ID");
                grilla.Columns.Add("Categoría", "Categoría");
                grilla.Columns.Add("Fecha y Hora", "Fecha y Hora");
                grilla.Columns.Add("Descripción", "Descripción");

                while (lectorBD.Read())
                {
                    //DatosExtraidos += (acumula)lectorBD[4] + "\n"(salto de linea);// // agrega los datos de la base de datos//
                    grilla.Rows.Add(lectorBD[1], lectorBD[2], lectorBD[3], lectorBD[4]);

                }
            }
            // hace que no se detenga el proyecto y siga funcionando//
            catch (Exception ex)
            {
                // si hay error, me lo muestra en error//
                Errores = ex.Message;
            }

        }

        public void TraerDatosDataSet(DataGridView grilla)
        {
            try
            {
                //conectamos a la base//
                ConectarBaseDatos();
                comandoBD = new OleDbCommand();

                comandoBD.Connection = conexionBD;
                comandoBD.CommandType = System.Data.CommandType.TableDirect;
                comandoBD.CommandText = "Registros";

                //crea en la memoria el adaptador//
                adaptadorDS = new OleDbDataAdapter(comandoBD);//con lo que el comando indique//
                //el adaptador rellena el DataSet//
                adaptadorDS.Fill(objDataSet, "Registros");


                //sirve el if para preguntar si tiene datos//
                if (objDataSet.Tables["Registros"].Rows.Count > 0)
                {
                    grilla.Columns.Add("ID", "ID");
                    grilla.Columns.Add("Categoría", "Categoría");
                    grilla.Columns.Add("Fecha y Hora", "Fecha y Hora");
                    grilla.Columns.Add("Descripción", "Descripción");


                    //para cada fila de la tabla Registros hacer...//
                    foreach (DataRow fila in objDataSet.Tables[0].Rows)
                    {
                        //DatosExtraidos += fila[1] + "\n";// //....grabar lo que está en la fila 0 de la columna0//

                        grilla.Rows.Add(fila[0], fila[1], fila[2], fila[3]);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }



        }
        public void RegistrarDatosDataSet(string ID)
        {
            try
            {
                //conectamos a la base//
                ConectarBaseDatos();
                comandoBD = new OleDbCommand();

                comandoBD.Connection = conexionBD;
                comandoBD.CommandType = System.Data.CommandType.TableDirect;
                comandoBD.CommandText = "Registros";

                //crea en la memoria el adaptador//
                adaptadorDS = new OleDbDataAdapter(comandoBD);//con lo que el comando indique//
                //el adaptador rellena el DataSet//
                adaptadorDS.Fill(objDataSet, "Registros");

                //carga y graba todo lo que trae el dataset de  Registros//
                DataTable TablaGrabar = objDataSet.Tables["Registros"];

                //Crea una nueva fila con todos los campos//
                DataRow filaGrabar = TablaGrabar.NewRow();

                //graba en cada caracter como si fuera caja de texto//
                filaGrabar[1] = ID;


                //en la tabla agrega el nuevo registro//
                TablaGrabar.Rows.Add(filaGrabar);


                //se crea un constructor para que pase a la base de datos//  
                OleDbCommandBuilder constructor = new OleDbCommandBuilder(adaptadorDS);

                //actualiza el DataSet con la nueva informacion//
                adaptadorDS.Update(objDataSet);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
