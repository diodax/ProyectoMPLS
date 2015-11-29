using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    static class ConexionSSH
    {
        private const string cPiUsername = "pi";
        private const string cPiPassword = "raspberry";
        private const string cScriptName = "OSPF_SNMP_Final.py";

        /// <summary>
        /// Establece la conexion SSH con el Raspberry, y retorna un string con los resultados del script
        /// </summary>
        /// <param name="cRaspberryIP"></param>
        /// <param name="nPuerto"></param>
        /// <param name="cRouterIP"></param>
        /// <param name="cCommunityString"></param>
        /// <returns></returns>
        public static string EjecutarOSPFDiscovery(string cRaspberryIP, int nPuerto, string cRouterIP, string cCommunityString)
        {
            string result = String.Empty;
            using (var client = new SshClient(cRaspberryIP, nPuerto, cPiUsername, cPiPassword))
            {
                client.Connect();
                var terminal = client.RunCommand(cScriptName + " " + cRouterIP + " " + cCommunityString);
                result = terminal.Result;
                client.Disconnect();
            }
            return result;
        }
    }
}