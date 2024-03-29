﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iOMG
{
    public class CCreditoCuota
    {
        public int NroCuota { get; set; }
        public int PlazoDiasCuota { get; set; }
        //[JsonConverter(typeof(FormatoFechaPersonalizado))]
        public string FechaVencimientoCuota { get; set; }
        public decimal MontoCuota { get; set; }

        public CCreditoCuota()
        {
            NroCuota = 0;
            PlazoDiasCuota = 0;
            FechaVencimientoCuota = "";     // DateTime.Now.ToString().Substring(0, 10);
            MontoCuota = 0;
        }
    }
}
