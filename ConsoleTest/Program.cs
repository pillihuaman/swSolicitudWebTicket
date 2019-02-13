using System;
using CoreWebLib;

using ConsoleTest.webticketinteragencias;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var lsolicitud = new Inserta_SolicitudEmisionRQ
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
                observacion = "Prueba de sistemas, por favor no prestarle atención",
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
                        //pagoTipo = "CASH",
                        pagoCash = "AAAAA. 00"
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
            };;

            int lcodigo = 0;

            try
            {
                var lservicio = new webticketinteragencias.webticketinteragencias();
                lcodigo = lservicio.InsertaSolicitud(lsolicitud);
                //var lcodigo = lservicio.InsertaSolicitud_TEST();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("código de solicitud generado: {0}", lcodigo);

            Console.ReadKey();
        }
    }
}
