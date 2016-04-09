using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models.Topologia
{
    /// <summary>
    /// Label Switch Path: Representa el camino entre un nodo y otro en la red MPLS
    /// </summary>
    public class LSP
    {
        public int idProyecto { get; set; }
        public int idLSP { get; set; }
        [Display(Name = "Nombre")]
        public string cNombre { get; set; }
        [Display(Name = "Ancho de Banda")]
        public double nBandwidth { get; set; }

        //Parametros de LSP_Header
        [Display(Name = "Nodo de Origen")]
        public int idRouterOrigen { get; set; }
        [Display(Name = "Próximo Salto")]
        public int idRouterNextHop { get; set; }
        [Display(Name = "Nodo de Destino")]
        public int idRouterDestino { get; set; }

        public int nSetupPriority { get; set; }
        public int nHoldPriority { get; set; }

        //Lista de idEnlaces para LSP_Details
        public List<Enlace> listaEnlaces { get; set; }
        public List<Router> listaNodos { get; set; }

        //SelectList con todos los nodos de la topologia. Estatico
        public List<SelectListItem> listaNodosOrigen { get; set; }
        //SelectList temporal con los nodos adyacentes al seleccionado
        public List<SelectListItem> listaNextHop { get; set; }
        

        public LSP() { }

        public LSP(int idProyecto)
        {
            //Populando los selectlists e inicializando las listas
            this.listaNodosOrigen = LSP.ConvertDropdownNodosDisponibles(LSP.SelectListaNodosDisponibles(idProyecto, null, 0));
            this.listaNextHop = new List<SelectListItem>();

            this.listaEnlaces = new List<Enlace>();
            this.listaNodos = new List<Router>();
        }

        public LSP(int idProyecto, int idLSP)
        {
            //Lee los datos de la tabla de LSP_Header
            Data.dsTopologiaTableAdapters.LSPsTableAdapter HeaderAdapter = new Data.dsTopologiaTableAdapters.LSPsTableAdapter();
            Data.dsTopologia.LSPsDataTable hdt = HeaderAdapter.SeleccionarListaLSPs(idProyecto, idLSP);

            foreach(var dr in hdt)
            {
                this.idProyecto = dr.idProyecto;
                this.idLSP = dr.idLSP;
                this.cNombre = dr.cNombre.Trim();
                this.nBandwidth = dr.nBandwidth;
                this.idRouterOrigen = dr.idRouterOrigen;
                this.idRouterDestino = dr.idRouterDestino;
                this.nSetupPriority = dr.nSetupPriority;
                this.nHoldPriority = dr.nHoldPriority;
            }

            //Agrega router_origen al stack de nodos
            Stack<Router> stackNodos = new Stack<Router>();
            stackNodos.Push(new LER(this.idRouterOrigen, this.idProyecto));
            

            //Bucle que agrega los routers de la lista de enlaces
            Data.dsTopologiaTableAdapters.EnlacesLSPsTableAdapter DetailAdapter = new Data.dsTopologiaTableAdapters.EnlacesLSPsTableAdapter();
            Data.dsTopologia.EnlacesLSPsDataTable ddt = DetailAdapter.SeleccionarListaEnlacesLSPs(idProyecto, idLSP); 

            foreach(var dr in ddt)
            {
                Enlace temp = new Enlace(dr.idLSP, dr.idProyecto);
                Router lastNode = stackNodos.Peek();

                //Agrega el router asociado al enlace que no esta en el tope del stack
                if (lastNode.idRouter != temp.idRouterA)
                    stackNodos.Push(new LSR(temp.idRouterA, this.idProyecto));
                else
                    stackNodos.Push(new LSR(temp.idRouterB, this.idProyecto));
            }

            //Finalmente, agrega router_destino al stack de nodos
            stackNodos.Push(new LER(this.idRouterDestino, this.idProyecto));

            //Convierte el stack a una lista
            this.listaNodos = stackNodos.ToList();

            //Populando los selectlists
            this.listaNodosOrigen = LSP.ConvertDropdownNodosDisponibles(LSP.SelectListaNodosDisponibles(idProyecto, null, 0));
            this.listaNextHop = new List<SelectListItem>();
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

        /// <summary>
        /// Devuelve la lista de nodos en un proyecto, o dado idRouter, la lista de nodos/enlaces adyacentes
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public static List<Router> SelectListaNodosDisponibles(int idProyecto, int? idRouter, int nBandwidthReservado)
        {
            List<Router> listaNodos = new List<Router>();
            Data.dsTopologiaTableAdapters.NodosAdyacentesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.NodosAdyacentesTableAdapter();
            Data.dsTopologia.NodosAdyacentesDataTable dt = Adapter.SeleccionarNodosAdyacentes(idProyecto, idRouter, nBandwidthReservado);

            foreach (var dr in dt)
            {
                LSR temp = new LSR(dr.idProyecto, dr.idRouter != idRouter ? dr.idRouter : dr.idRouter2);
                temp.listaEnlaces = new List<Enlace>();
                temp.listaEnlaces.Add(new Enlace(dr.idEnlace, dr.idProyecto));
                listaNodos.Add(temp);
            }
            return listaNodos;
        }

        public static List<SelectListItem> ConvertDropdownNodosDisponibles(List<Router> listaNodos)
        {
            List<SelectListItem> dpNodos = new List<SelectListItem>();
            foreach (var item in listaNodos)
            {
                SelectListItem temp = new SelectListItem();
                temp.Value = item.idRouter.ToString();
                temp.Text = item.cHostname.ToString() + "[" + item.cRouterID.Trim() + "]";
                dpNodos.Add(temp);
            }
            return dpNodos;
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