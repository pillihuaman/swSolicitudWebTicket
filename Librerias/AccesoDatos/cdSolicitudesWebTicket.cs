using System;
using System.Linq;
using System.Data;

using Oracle.DataAccess.Client;

using CustomLog;

using AccesoDatos.NMOracle;

namespace AccesoDatos
{
    public partial class cdSolicitudesWebTicket
    {
        public int InsertaSolicitud(Inserta_SolicitudEmisionRQ obj)
        {
            var nmOracle = new Conexion();
            var intSolicitud = 0;
            OracleConnection connection;

            using (connection = new OracleConnection(nmOracle.strCadena))
            {
                if (nmOracle.Connect(connection))
                {
                    var objTx = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    nmOracle.Transaction(objTx);

                    try
                    {
                        nmOracle.SP_Command(nmOracle.Esquema + ".PKG_SOLICITUD.SP_SOL_INSERTA2", nmOracle.strStoredProcedure);

                        nmOracle.AgregarParametro("pNumIdWeb_in", obj.idWeb, OracleDbType.Int32, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumIdLang_in", obj.idLang, OracleDbType.Int32, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumIdUsuWeb_in", obj.idUsuWeb, OracleDbType.Int32, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarPnrCod_in", obj.pnrCod, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarTipoReserva_in", obj.tipoReserva, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarPromotor_in", obj.promotor, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumTarfBruta_in", obj.tarfBruta, OracleDbType.Decimal, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumIgv_in", obj.igv, OracleDbType.Decimal, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumPorcentaje_in", obj.porcentaje, OracleDbType.Decimal, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumQueues_in", obj.queues, OracleDbType.Decimal, ParameterDirection.Input); //0
                        nmOracle.AgregarParametro("pNumOtrosPagos_in", obj.otrosPagos, OracleDbType.Decimal, ParameterDirection.Input); //0
                        nmOracle.AgregarParametro("pNumIncentivos_in", obj.incentivos, OracleDbType.Decimal, ParameterDirection.Input); //0
                        nmOracle.AgregarParametro("pNumTarfNeta_in", obj.tarfNeta, OracleDbType.Decimal, ParameterDirection.Input); //0
                        nmOracle.AgregarParametro("pVarObservacion_in", obj.observacion, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumEstado_in", obj.estado, OracleDbType.Int32, ParameterDirection.Input); //0
                        nmOracle.AgregarParametro("pNumIdOfiDestino_in", obj.idOfiDestino, OracleDbType.Int32, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarTipoTarifa_in", obj.tipoTarifa, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumIdUsuWebSeg_in", obj.idUsuWebSeg, OracleDbType.Int32, ParameterDirection.Input); //-1
                        nmOracle.AgregarParametro("pChrDoc1_in", obj.doc1, OracleDbType.Char, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarNroDoc1_in", obj.nroDoc1, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pChrDoc2_in", obj.doc2, OracleDbType.Char, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarNroDoc2_in", obj.nroDoc2, OracleDbType.Varchar2, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumIdUsuWebProc_in", obj.idUsuWebProc, OracleDbType.Int32, ParameterDirection.Input); //-1
                        nmOracle.AgregarParametro("pNumIdDepDestino_in", obj.idDepDestino, OracleDbType.Int32, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pVarNomPagador_in", obj.nomPagador, OracleDbType.Varchar2, ParameterDirection.Input); //""
                        nmOracle.AgregarParametro("pVarApePagador_in", obj.apePagador, OracleDbType.Varchar2, ParameterDirection.Input); //""
                        nmOracle.AgregarParametro("pChrPaisDestino_in", obj.paisDestino, OracleDbType.Char, ParameterDirection.Input);
                        nmOracle.AgregarParametro("pNumSistOrigen_in", obj.sistemaOrigen, OracleDbType.Int16, ParameterDirection.Input); //1
                        nmOracle.AgregarParametro("pChrSabreAmadeus_in", obj.sabreAmadeus, OracleDbType.Char, ParameterDirection.Input);

                        nmOracle.AgregarParametro("pID_PUNTO_EMISION_PTA_in", obj.idPuntoEmision, OracleDbType.Int16, ParameterDirection.Input); //null
                        nmOracle.AgregarParametro("pID_SUCURSAL_EMISION_PTA_in", obj.idsucursalEmision, OracleDbType.Int16, ParameterDirection.Input); //null
                        nmOracle.AgregarParametro("pNumConWaiver_in", obj.conWaiver, OracleDbType.Int16, ParameterDirection.Input); //0
                        nmOracle.AgregarParametro("pNumIdSubcodigo_in", obj.idSubCodigo, OracleDbType.Int16, ParameterDirection.Input); //0

                        nmOracle.AgregarParametro("pFileReferencia_in", obj.fileReferencia, OracleDbType.Varchar2, ParameterDirection.Input); //null

                        nmOracle.AgregarParametro("pNumIdNewSol_out", null, OracleDbType.Int32, ParameterDirection.Output);

                        nmOracle._ExecuteNonQuery(false);

                        var queryLog = nmOracle.objSBQuery;

                        intSolicitud = int.Parse(nmOracle.LeeParametros("pNumIdNewSol_out", null));

                        if (obj.pasajeros != null)
                        {
                            foreach (var item in obj.pasajeros)
	                        {
                                InsertaPasajeros(item, intSolicitud, obj.idUsuWeb, obj.strNombrePagina, obj.idLang, obj.idWeb, connection, objTx);
	                        }
                        }

                        if (obj.itinerarios != null)
                        {
                            if (obj.itinerarios.Any(a => !string.IsNullOrEmpty(a.familyFare)))
                            {
                                foreach (var item in obj.itinerarios)
                                {
                                    InsertSegmento2(item, intSolicitud, obj.idUsuWeb, obj.strNombrePagina, obj.idLang, obj.idWeb, connection, objTx);
                                }

                            }
                            else
                            {
                                foreach (var item in obj.itinerarios)
                                {
                                    InsertSegmento(item, intSolicitud, obj.idUsuWeb, obj.strNombrePagina, obj.idLang, obj.idWeb, connection, objTx);
                                }                            
                            }
                            
                        }

                        if (obj.pagos != null)
                        {
                            foreach (var item in obj.pagos)
                            {
                                InsertaFormaPago(item, intSolicitud, obj.idUsuWeb, obj.strNombrePagina, obj.idLang, obj.idWeb, connection, objTx);
                            }
                        }

                        Inserta(obj.idUsuWeb, obj.strNombrePagina, string.Empty, obj.idLang, obj.idWeb, queryLog, connection, objTx);
                       
                        objTx.Commit();

                    }
                    catch (Exception ex)
                    {
                        // registrando evento
                        Bitacora.Current.Error<cdSolicitudesWebTicket>(ex, new { obj });

                        //intSolicitud = 0;
                        objTx.Rollback();
                        throw;
                    }
                }
            }

            return intSolicitud;
        }

        private void InsertaPasajeros(SolicitudPasajero obj, 
                                      int intSolicitud, 
                                      int idUsuWeb, 
                                      string strNombrePagina, 
                                      int idLang, 
                                      int idWeb, 
                                      OracleConnection connection, 
                                      OracleTransaction objTx) 
        {
            var nmOracle = new Conexion();

            nmOracle.SP_Command(nmOracle.Esquema + ".PKG_SOLICITUD.SP_SOL_PASAJERO_INSERTA", nmOracle.strStoredProcedure);
         
            nmOracle.AgregarParametro("pNumIdNewSol", intSolicitud, OracleDbType.Double, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarTipoPasajero", obj.tipoPasajero, OracleDbType.Varchar2,ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNombrePasajero", obj.nombrePasajero, OracleDbType.Varchar2,  ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNroDocumento", obj.nroDocumento, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarTipoDocumento", obj.tipoDocumento, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarFecNac", obj.fechaNacimiento, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrSexo", obj.sexo, OracleDbType.Char, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNumPax_in", obj.strNumeroPax, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarSegNomPax_in", string.Empty, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNacDoc_in", string.Empty,OracleDbType.Varchar2,  ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarPaisEmisorDoc_in", string.Empty,OracleDbType.Varchar2,  ParameterDirection.Input);
            nmOracle.AgregarParametro("pDatFecExpDoc_in", string.Empty,OracleDbType.Varchar2,  ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNumRUCEmite", obj.strNumeroRUC, OracleDbType.Varchar2, ParameterDirection.Input);

            nmOracle.Transaction(objTx);
            nmOracle._ExecuteNonQuery(false);

            Inserta(idUsuWeb, strNombrePagina, string.Empty, idLang, idWeb, nmOracle.objSBQuery, connection, objTx);     
        }

        private void InsertSegmento2(SolicitudItinerario obj, 
                                     int intSolicitud, 
                                     int idUsuWeb, 
                                     string strNombrePagina, 
                                     int idLang, 
                                     int idWeb, 
                                     OracleConnection connection,
                                     OracleTransaction objTx)
        {
            var nmOracle = new Conexion();

            nmOracle.SP_Command(nmOracle.Esquema + ".PKG_SOLICITUD.SP_SOL_ITINERARIO_INSERTA2", nmOracle.strStoredProcedure);

            nmOracle.AgregarParametro("pNumIdNewSol", intSolicitud, OracleDbType.Double, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrLineaAerea", obj.lineaAerea, OracleDbType.Char, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNroVuelo", obj.nroVuelo, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarClase", obj.clase, OracleDbType.Varchar2,ParameterDirection.Input);
            nmOracle.AgregarParametro("pDatFechaVuelo", obj.fechaVuelo, OracleDbType.Date, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrOrigenVuelo", obj.origenVuelo, OracleDbType.Char, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrDestinoVuelo", obj.destinoVuelo, OracleDbType.Char, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrFamilyFare", obj.familyFare, OracleDbType.Varchar2, ParameterDirection.Input);

            nmOracle.Transaction(objTx);
            nmOracle._ExecuteNonQuery(false);

            Inserta(idUsuWeb, strNombrePagina, string.Empty, idLang, idWeb, nmOracle.objSBQuery, connection, objTx); 
        }

        private void InsertSegmento(SolicitudItinerario obj, 
                                    int intSolicitud, 
                                    int idUsuWeb, 
                                    string strNombrePagina, 
                                    int idLang, 
                                    int idWeb, 
                                    OracleConnection connection, 
                                    OracleTransaction objTx)
        {
            var nmOracle = new Conexion();

            nmOracle.SP_Command(nmOracle.Esquema + ".PKG_SOLICITUD.SP_SOL_ITINERARIO_INSERTA", nmOracle.strStoredProcedure);

            nmOracle.AgregarParametro("pNumIdNewSol", intSolicitud, OracleDbType.Double, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrLineaAerea", obj.lineaAerea, OracleDbType.Char, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNroVuelo", obj.nroVuelo, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarClase", obj.clase, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pDatFechaVuelo", obj.fechaVuelo, OracleDbType.Date, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrOrigenVuelo", obj.origenVuelo, OracleDbType.Char, ParameterDirection.Input);
            nmOracle.AgregarParametro("pChrDestinoVuelo", obj.destinoVuelo, OracleDbType.Char, ParameterDirection.Input);

            nmOracle.Transaction(objTx);
            nmOracle._ExecuteNonQuery(false);

            Inserta(idUsuWeb, strNombrePagina, string.Empty, idLang, idWeb, nmOracle.objSBQuery, connection, objTx);
        }

        private void InsertaFormaPago(SolicitudPago obj, 
                                      int intSolicitud, 
                                      int idUsuWeb, 
                                      string strNombrePagina, 
                                      int idLang, 
                                      int idWeb, 
                                      OracleConnection connection, 
                                      OracleTransaction objTx)
        { 
            var nmOracle = new Conexion();

            nmOracle.SP_Command(nmOracle.Esquema + ".PKG_SOLICITUD.SP_SOL_PAGO_INSERTA", nmOracle.strStoredProcedure);

            nmOracle.AgregarParametro("pNumIdNewSol", intSolicitud, OracleDbType.Double, ParameterDirection.Input);
            nmOracle.AgregarParametro("pNumIdPaisTarjeta", obj.idPaisTarjeta, OracleDbType.Int32, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarIdDocumento", obj.idDocumento, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarPagoTipo", obj.pagoTipo, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarPagoCash", obj.pagoCash, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarPagoTarjeta", obj.pagoTarjeta, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarPagoTipoTarjeta", obj.pagoTipoTarjeta, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNroTarjeta", obj.nroTarjeta, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarTitularTarjeta", obj.titularTarjeta, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNroDocumento", obj.nroDocumento, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarBancoTitularTarjeta", obj.bancoTitularTarjeta, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pDatFechVenTarjeta", obj.fechVenTarjeta, OracleDbType.Date, ParameterDirection.Input);
            nmOracle.AgregarParametro("pNumIdOficina", obj.idiOfi, OracleDbType.Int16, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNumCuenta", obj.nroCuenta, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNumOperacion", obj.nroOperacion, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarMonto", obj.monto, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pDatFechDeposito",obj.fechDeposito, OracleDbType.Date, ParameterDirection.Input);
            nmOracle.AgregarParametro("pNumIdOficina2", obj.idiOfi2, OracleDbType.Int32, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNumCuenta2", obj.nroCuenta2, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNumOperacion2", obj.nroOperacion2, OracleDbType.Varchar2,ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarMonto2", obj.monto2, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pDatFechDeposito2", obj.fechDeposito2, OracleDbType.Date, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNomBanco", obj.strNombreBanco, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNomBanco2", obj.strNombreBanco, OracleDbType.Varchar2, ParameterDirection.Input);
            //código seguridad de la tarjeta
            nmOracle.AgregarParametro("pVarPagCodSegTarj_in", obj.strCodSeguridad, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNomSucBanco_in", obj.strSucursalBanco, OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarReferencia_in", obj.strReferenciaDeposito,OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNomImagen_in", obj.strNombreArchivo, OracleDbType.Varchar2, ParameterDirection.Input);

            nmOracle.Transaction(objTx);
            nmOracle._ExecuteNonQuery(false);

            Inserta(idUsuWeb, strNombrePagina, string.Empty, idLang, idWeb, nmOracle.objSBQuery, connection, objTx);   
        }
    }
}
