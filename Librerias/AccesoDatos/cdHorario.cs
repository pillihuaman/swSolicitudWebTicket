using System;
using System.Data;

using Oracle.DataAccess.Client;

using AccesoDatos.NMOracle;

namespace AccesoDatos
{
    public class cdHorario
    {
        public HorarioRS HorarioEasy (HorarioRQ objHorarioEasy) 
        {
            HorarioRS horarioRS = null;
            var nmOracle = new Conexion();
            OracleConnection connection;

            using (connection = new OracleConnection(nmOracle.strCadena))
            {
                if (nmOracle.Connect(connection,false))
                {
                    nmOracle.SP_Command(nmOracle.Esquema + ".PKG_GDS_HORARIOS.GDS_NEW_INGRESO_EASY", nmOracle.strStoredProcedure);
                    nmOracle.AgregarParametro("p_TIPO_CLIENTE", objHorarioEasy.eCondicionAgy.ToString(), OracleDbType.Varchar2, ParameterDirection.Input);
                    nmOracle.AgregarParametro("p_WEBS_CID", objHorarioEasy.intIdWeb, OracleDbType.Int16, ParameterDirection.Input);
                    nmOracle.AgregarParametro("p_MODULO",Convert.ToInt16(objHorarioEasy.eModuloEasy), OracleDbType.Int16, ParameterDirection.Input);
                    nmOracle.AgregarParametro("p_OPC_ID", objHorarioEasy.intOpcion, OracleDbType.Int16, ParameterDirection.Input); 
                    nmOracle.AgregarParametro("p_Ingresa", null, OracleDbType.Int32, ParameterDirection.Output);
                    nmOracle.AgregarParametro("p_Solicitud", null, OracleDbType.Int32, ParameterDirection.Output);

                    nmOracle._ExecuteNonQuery(false);

                    horarioRS = new HorarioRS
                    {
                        intPermitirAutomatica = int.Parse(nmOracle.LeeParametros("p_Ingresa", "-1")),
                        intPermitirCounter = int.Parse(nmOracle.LeeParametros("p_Solicitud", "-1"))
                    };
                }
            }

            return horarioRS;
        }
    }
}
