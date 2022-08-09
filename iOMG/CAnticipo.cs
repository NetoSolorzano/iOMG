using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CAnticipo
    {
        public int ComprobanteID { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string Serie { get; set; }
        public int Correlativo { get; set; }
        public string MonedaCodigo { get; set; }
        public string AnticiposId { get; set; }
        public decimal Anticipo { get; set; }
        public decimal AnticipoBase { get; set; }
        public decimal AnticipoIGV { get; set; }
        public string TipoAfectacionIGVCodigo { get; set; }
        public string TipoDocIdentidadCodigo { get; set; }
        public string NumeroDocIdentidad { get; set; }

        public CAnticipo()
        {
            ComprobanteID = 0;
            TipoDocumentoCodigo = "";
            Serie = "";
            Correlativo = 0;
            MonedaCodigo = "";
            AnticiposId = "";
            Anticipo = 0;
            AnticipoBase = 0;
            AnticipoIGV = 0;
            TipoDocIdentidadCodigo = "";
            NumeroDocIdentidad = "";
            TipoAfectacionIGVCodigo = "";
        }
    }
}
