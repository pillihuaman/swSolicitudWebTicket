using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Web.Services;

using AccesoDatos;

namespace swSolicitudes
{
    /// <summary>
    /// Summary description for webticketinteragencias
    /// </summary>
    [WebService(Namespace = "http://webticketinteragencias.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class webticketinteragencias : WebService
    {
        [WebMethod]
        public int InsertaSolicitud(Inserta_SolicitudEmisionRQ obj)
        {
            var x1 = 2;
            var x2 = 0;

            //try
            //{
            //    var s1 = x1 / x2;

            //}
            //catch (Exception ex)
            //{
            //    Console.Out.Write(ex.Message);
            //}

            var s2 = x1/x2;

            foreach (var lpago in obj.pagos)
            {
                lpago.pagoTipo = Regex.Replace(lpago.pagoTipo, @"\s+", " ", RegexOptions.Multiline);
            }

            return new cdSolicitudesWebTicket().InsertaSolicitud(obj);
        }

        [WebMethod]
        public HorarioRS HorarioEasy(HorarioRQ obj)
        {
            return new cdHorario().HorarioEasy(obj);
        }

        [WebMethod]
        public Planilla CodigoPlanilla(string strUsuWebLogin)
        {
            return new cdPlanilla().CodigoPlanilla(strUsuWebLogin);
        }

        [WebMethod]
        public HorarioRS HorarioEasy_TEST()
        {
            var obj = new HorarioRQ {
                eCondicionAgy = CondicionAgy.CON,
                intIdWeb = 3,
                eModuloEasy = ModuloEasy.Emision,
                intOpcion = 2307
            };

            return new cdHorario().HorarioEasy(obj);
        }

        [WebMethod]
        public int InsertaSolicitud_TEST()
        {
            var obj = new Inserta_SolicitudEmisionRQ()
            {
                idWeb = 3,
                idLang = 1,
                idOfiDestino = 8,
                idDepDestino = 6,
                sistemaOrigen = 1,

                strNombrePagina = "E-asy! OnLine",

                queues = 0,
                otrosPagos = 0,
                incentivos = 0,
                estado = 0,
                idUsuWebSeg = -1,
                idUsuWebProc = -1,
                nomPagador = string.Empty,
                apePagador = string.Empty,

                idUsuWeb = 10796,
                pnrCod = "PRUEBA",
                tipoReserva = "Sabre",
                sabreAmadeus = "S",
                promotor = "Telemarketing",
                tarfBruta = 500.00,
                tarfNeta = 0,
                igv = 0.00,
                porcentaje = 0,
                idSubCodigo = 0,
                observacion = "Prueba de sistemas",
                tipoTarifa = "IT",
                doc1 = "FC",
                doc2 = "BB",
                nroDoc2 = "12345",
                idPuntoEmision = 14,
                idsucursalEmision = 2,
                conWaiver = 0,
                fileReferencia = "123-456789",
         
                pasajeros = new[]
                { 
                    new SolicitudPasajero
                    {
                        strNumeroPax = "1.1", 
                        nombrePasajero = "SANCHEZ HUGO",
                        tipoPasajero = "AD",
                        fechaNacimiento = "04/08/1978",
                        sexo = "M",
                        tipoDocumento = "DNI",
                        nroDocumento = "15451652",
                        strNumeroRUC = "10154516528"
                    },

                    new SolicitudPasajero
                    {
                        strNumeroPax = "2.1", 
                        nombrePasajero = "SANCHEZ LUIS",
                        tipoPasajero = "C",
                        fechaNacimiento = "04/08/1975",
                        sexo = "M",
                        tipoDocumento = "DNI",
                        nroDocumento = "15451654",
                        strNumeroRUC = "10154516529"
                    }
                },

                itinerarios = new[] 
                {
                    new SolicitudItinerario
                    { 
                        lineaAerea = "LA",
                        nroVuelo = "2505",
                        clase = "Y",
                        origenVuelo = "LIM",
                        fechaVuelo = new DateTime(2017,03,24),
                        destinoVuelo = "MIA",
                        //familyFare = "SL"
                    },

                    new SolicitudItinerario
                    { 
                        lineaAerea = "LA",
                        nroVuelo = "2508",
                        clase = "Y",
                        origenVuelo = "MIA",
                        fechaVuelo = new DateTime(2017,03,30),
                        destinoVuelo = "LIM",
                        familyFare = "SL"
                    }
                },

                pagos = new[]
                {
                    new SolicitudPago
                    {
                        //pagoTipo = "\\r\\n                                         Se enviará información antes de las 16:00\\r\\n                                    ",
                        pagoTipo = "CASH",
                        pagoCash = "200.00"
                    },

                    new SolicitudPago
                    {
                        pagoTipo = "TARJETA DE CREDITO",
                        pagoTarjeta = "300.00",
                        bancoTitularTarjeta = "BCP",
                        pagoTipoTarjeta = "VI",
                        nroTarjeta = "1234567890123456",
                        fechVenTarjeta = new DateTime(2017,11,01),
                        idPaisTarjeta = 52,
                        titularTarjeta = "HUGO SANCHEZ",
                        idDocumento = "DNI",
                        nroDocumento = "15451652"
                    }
                }
            };

            //return new cdSolicitudesWebTicket().InsertaSolicitud(obj);
            return InsertaSolicitud(obj);
        }
    }
}
