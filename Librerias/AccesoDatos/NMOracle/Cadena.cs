using System.Text;

using Oracle.DataAccess.Client;

namespace AccesoDatos.NMOracle
{
    public partial class Conexion
    {

        private OracleCommand objOracleCommand = new OracleCommand();
        private OracleTransaction objOracleTransaction;
        //private OracleDataAdapter objOracleDataAdapter = new OracleDataAdapter();

        public StringBuilder objSBQuery = new StringBuilder();

        private const int intCommandTimeout = 180;
        public string strStoredProcedure = "SP";
        public string strSentenciaText = "TX";

        private const string IP = "10.75.102.15";
        private const string Puerto = "1521";
        private const string Usuario = "appwebs";
        private const string Contraseña = "6109338";

        public string Esquema = "APPWEBS";
        public string EsquemaDemo = "DEMOAPPWEBS";

        public string strCadena = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + IP + ")" +
                "(PORT=" + Puerto + ")))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));User Id=" + Usuario + ";Password=" + Contraseña;


        public string strCadenaDemo = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + IP + ")" +
        "(PORT=" + Puerto + ")))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));User Id=demo" + Usuario + ";Password=" + Contraseña;
    }
}
