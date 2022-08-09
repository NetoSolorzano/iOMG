using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CDocumentoRelacionado
    {
        public int ID { get; set; }
        public int Relacion { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string Serie { get; set; }
        public int Correlativo { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Moneda { get; set; }
        public decimal Importe { get; set; }
        public string UsuarioNombre { get; set; }
        public string IDPadre { get; set; }
        public string PDF { get; set; }

        public CDocumentoRelacionado()
        {
            ID = 0;
            Relacion = 0;
            TipoDocumentoCodigo = "";
            Serie = "";
            Correlativo = 0;
            FechaEmision = DateTime.Now;
            Moneda = "";
            Importe = 0;
            IDPadre = "";
            UsuarioNombre = "";
        }

        public CDocumentoRelacionado(int pID, int pRelacion, string pTipoDocumentoCodigo, string pSerie, int pCorrelativo, DateTime pFechaEmision, string pMoneda, decimal pImporte, string pIDPadre)
        {
            ID = pID;
            Relacion = pRelacion;
            TipoDocumentoCodigo = pTipoDocumentoCodigo;
            Serie = pSerie;
            Correlativo = pCorrelativo;
            FechaEmision = pFechaEmision;
            Moneda = pMoneda;
            Importe = pImporte;
            IDPadre = pIDPadre;
        }
    }
}
