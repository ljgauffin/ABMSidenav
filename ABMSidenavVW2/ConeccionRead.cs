using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMSidenavVW2
{

    public class QueryExecutor //clase para manejo de conceccion a sql
    {
        private string stringConexion = "Data Source= 172.17.2.18;Initial Catalog=Poas2000;User ID=SA;Password=kpL40Sis23";
        private SqlConnection conexionConnection;
        private SqlCommand conexionCommand;
        private SqlDataReader conexionDataReader;
        private SqlDataReader reader;

        public SqlDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }
        public QueryExecutor()
        {
            conexionConnection = new SqlConnection(stringConexion);
        }

        public DataTable ConectDBdt(string query)
        {
            DataTable dt = new DataTable();
            conectar();
            conexionCommand.CommandText = query;
            dt.Load(conexionCommand.ExecuteReader());

            conexionConnection.Close();

            return dt;

        }


        public void ConectDBreader(string query)
        {

            conectar();
            conexionCommand.CommandText = query;
            Reader = conexionCommand.ExecuteReader();

            //conexionConnection.Close();

        }

        private void conectar()
        {
            conexionConnection.Open();
            conexionCommand = new SqlCommand();
            conexionCommand.Connection = conexionConnection;
            conexionCommand.CommandType = CommandType.Text;

        }

        public void desconectar()
        {
            conexionConnection.Close();
        }
        public SqlDataReader ExecuteQuery(string query) //recibe un query, abre la conceccion, la cierra y devuelve el reader
        {
            SqlConnection connection = new SqlConnection(stringConexion);
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
