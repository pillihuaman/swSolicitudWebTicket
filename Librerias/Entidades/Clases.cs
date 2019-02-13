using System;

public class Signature 
{
	public short idWeb { set; get; }
	public short idLang { set; get; }
	public string ip { set; get; }
	public string keyAccess { set; get; }
	public string usuario { set; get; }
	public short idSecuencia { set; get; }
	public string browser { set; get; }
	public string rUC { set; get; }
	public string razonSocial { set; get; }
	public short idDispositivo { set; get; }
}

public class SolicitudItinerario 
{
	public string lineaAerea { set; get; }
	public string nroVuelo { set; get; }
	public string clase { set; get; }
	public DateTime fechaVuelo { set; get; }
	public string origenVuelo { set; get; }
	public string destinoVuelo { set; get; }
    public string familyFare { set; get; }
}

public class SolicitudPasajero 
{
	public string tipoPasajero { set; get; }
	public string nombrePasajero { set; get; }
	public string nroDocumento { set; get; }
	public string tipoDocumento { set; get; }
	public string tipoDocumentoSabre { set; get; }
	public string documentoSabre { set; get; }
	public string fechaNacimiento { set; get; }
	public string sexo { set; get; }
    public string strNumeroPax { set; get; }
    public string strNumeroRUC { set; get; }
}

public class SolicitudPago 
{
	public int idPago { set; get; }
	public string pagoTipo { set; get; }
	public string pagoCash { set; get; }
	public string pagoTarjeta { set; get; }
	public string pagoTipoTarjeta { set; get; }
	public string nroTarjeta { set; get; }
	public DateTime? fechVenTarjeta { set; get; }
	public string titularTarjeta { set; get; }
	public int idPaisTarjeta { set; get; }
	public string bancoTitularTarjeta { set; get; }
	public string idDocumento { set; get; }
	public string nroDocumento { set; get; }
	public int idiOfi { set; get; }
	public string nroCuenta { set; get; }
	public string nroOperacion { set; get; }
	public string monto { set; get; }
	public DateTime? fechDeposito { set; get; }
	public int idiOfi2 { set; get; }
	public string nroCuenta2 { set; get; }
	public string nroOperacion2 { set; get; }
	public string monto2 { set; get; }
    public DateTime? fechDeposito2 { set; get; }

    public string strNombreBanco { set; get; }
    public string strNombreBanco2 { set; get; }
    public string strCodSeguridad { set; get; }

    public string strSucursalBanco { set; get; }
    public string strReferenciaDeposito { set; get; }
    public string strNombreArchivo { set; get; }
}
[Serializable]
public class Inserta_SolicitudEmisionRQ 
{
	public string iPUsuario { set; get; }
	public Signature signature { set; get; }
	public int idWeb { set; get; }
	public int idLang { set; get; }
	public int idUsuWeb { set; get; }
	public string pnrCod { set; get; }
	public string tipoReserva { set; get; }
	public string promotor { set; get; }
	public double tarfBruta { set; get; }
	public double igv { set; get; }
	public double porcentaje { set; get; }
	public double queues { set; get; }
	public double otrosPagos { set; get; }
	public double incentivos { set; get; }
	public double tarfNeta { set; get; }
	public string observacion { set; get; }
	public int estado { set; get; }
	public int idOfiDestino { set; get; }
	public string tipoTarifa { set; get; }
	public int idUsuWebSeg { set; get; }
	public string doc1 { set; get; }
	public string nroDoc1 { set; get; }
	public string doc2 { set; get; }
	public string nroDoc2 { set; get; }
	public int idUsuWebProc { set; get; }
	public int idDepDestino { set; get; }
	public string nomPagador { set; get; }
	public string apePagador { set; get; }
	public SolicitudItinerario[] itinerarios { set; get; }
	public SolicitudPago[] pagos { set; get; }
	public SolicitudPasajero[] pasajeros { set; get; }
	public string paisDestino { set; get; }
	public string sabreAmadeus { set; get; }
	public short sistemaOrigen { set; get; }
	public string nomBanco { set; get; }
	public string nomBanco2 { set; get; }
	public string codSeguridadTarj { set; get; }
	public int? idPuntoEmision { set; get; }
	public int? idsucursalEmision { set; get; }
	public int? conWaiver { set; get; }
	public int? idSubCodigo { set; get; }
    public string fileReferencia { set; get; }
	public string nomSucBanco { set; get; }
	public string referencia { set; get; }
	public string nomImagen { set; get; }
	public string enviaDeposito { set; get; }
    public string strNombrePagina { set; get; }
}
