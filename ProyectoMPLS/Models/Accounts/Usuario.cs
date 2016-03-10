using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ProyectoMPLS.Models.Accounts
{
    public class Usuario
    {
        public string cUserName { get; set; }
        public string cEmail { get; set; }
        public string cPassword { get; set; }


        public Usuario() { }

        /// <summary>
        /// Registra un nuevo usuario, antes de haber establecido la confirmación por correo
        /// </summary>
        /// <param name="usuario"></param>
        public void InsertarUsuario(RegistroViewModel usuario)
        {
            var hashedPassword = Crypto.HashPassword(usuario.cPassword);

            Data.dsUsuariosTableAdapters.Operaciones op = new Data.dsUsuariosTableAdapters.Operaciones();
            op.InsertarUsuario(usuario.cUserName, usuario.cEmail, false, hashedPassword);
        }

        /// <summary>
        /// Determina si el nombre de usuario ya existe o no en la DB
        /// </summary>
        /// <param name="cUserName"></param>
        /// <returns></returns>
        public static bool ExisteUsuario(string cUserName)
        {
            Data.dsUsuariosTableAdapters.Operaciones op = new Data.dsUsuariosTableAdapters.Operaciones();
            return (bool)op.ExisteUsuario(cUserName);
        }

        /// <summary>
        /// Determina si el usuario y la contraseña son válidos en la DB
        /// </summary>
        /// <param name="cUserName"></param>
        /// <param name="cEnteredPassword"></param>
        /// <returns></returns>
        public static bool EsLoginValido(string cUserName, string cEnteredPassword)
        {
            Data.dsUsuariosTableAdapters.Operaciones op = new Data.dsUsuariosTableAdapters.Operaciones();
            string cPasswordHash = op.SelectHash(cUserName).ToString();
            var doesPasswordMatch = Crypto.VerifyHashedPassword(cPasswordHash, cEnteredPassword);
            return doesPasswordMatch;

            //return (bool)op.EsClaveValida(cUserName, cPasswordHash);
        }

    }
}