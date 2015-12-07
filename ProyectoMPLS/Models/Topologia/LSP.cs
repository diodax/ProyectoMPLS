using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    /// <summary>
    /// Label Switch Path: Representa el camino entre un nodo y otro en la red MPLS
    /// </summary>
    public class LSP
    {
        public int idProyecto { get; set; }
        public int idLSP { get; set; }
        public string cNombre { get; set; }
        public double nBandwidth { get; set; }
        
        //Nuevos parametros
        public int idRouterOrigen { get; set; }
        public int idRouterDestino { get; set; }

        public int nSetupPriority { get; set; }
        public int nHoldPriority { get; set; }

        public List<Enlace> listaEnlaces { get; set; }
        public List<Router> listaNodos { get; set; }

        public LSP() { }

        public LSP(int idProyecto)
        {
            //
        }

        public LSP(int idProyecto, int idLSP)
        {
            //
        }

        public static List<LSP> SelectListaLSP(int idProyecto)
        {
            List<LSP> listaLSP = new List<LSP>();
            Data.dsTopologiaTableAdapters.LSPsTableAdapter Adapter = new Data.dsTopologiaTableAdapters.LSPsTableAdapter();
            Data.dsTopologia.LSPsDataTable dt = Adapter.SeleccionarListaLSPs(idProyecto, null);

            foreach(var dr in dt)
            {
                LSP temp = new LSP();
                temp.idProyecto = dr.idProyecto;
                temp.idLSP = dr.idLSP;
                if (!dr.IscNombreNull())
                    temp.cNombre = dr.cNombre.Trim();
                if (!dr.IsnBandwidthNull())
                    temp.nBandwidth = dr.nBandwidth;
                temp.idRouterOrigen = dr.idRouterOrigen;
                temp.idRouterDestino = dr.idRouterDestino;
                if (!dr.IsnSetupPriorityNull())
                    temp.nSetupPriority = dr.nSetupPriority;
                if (!dr.IsnHoldPriorityNull())
                    temp.nHoldPriority = dr.nHoldPriority;

                listaLSP.Add(temp);
            }

            return listaLSP;
        }

        public static List<Router> SelectListaNodos(int idProyecto)
        {
            List<Router> listaNodos = new List<Router>();
            return listaNodos;
        }
           
        public static List<Enlace> SelectListaEnlacesValidos(int idProyecto, int idRouter, double nBandwidth)
        {
            List<Enlace> listaEnlaces = new List<Enlace>();
            return listaEnlaces;
        }
    }

    public class Nodo
    {
        //
    }
}