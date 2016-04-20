using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Estadisticas
{
    public class SettingsConsumo
    {
        public int idProyecto { get; set; }

        public double nLimiteMenor { get; set; }
        public double nLimiteMayor { get; set; }

        public string cColorInferior { get; set; }
        public string cColorMedio { get; set; }
        public string cColorSuperior { get; set; }

        public SettingsConsumo() { }

        public SettingsConsumo(int idProyecto)
        {
            this.nLimiteMenor = 0.3;    //30%
            this.nLimiteMayor = 0.6;    //60%

            this.cColorInferior = "#00FF00";    //Default Green
            this.cColorMedio = "#FFFF00";    //Default Yellow
            this.cColorSuperior = "#FF0000";    //Default Red
        }
    }
}