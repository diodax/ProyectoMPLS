using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ProyectoMPLS.Models.Accounts
{
    public class RegistroViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string cUserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string cEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string cPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("cPassword", ErrorMessage = "La contraseña y la confirmación de ésta no coinciden.")]
        public string cConfirmarPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string cUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string cPassword { get; set; }

        [Display(Name = "¿Mantenerme Conectado?")]
        public bool bRecordarUsuario { get; set; }
    }

    public class SettingsViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string cUserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string cEmail { get; set; }

        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string cPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("cPassword", ErrorMessage = "La contraseña y la confirmación de ésta no coinciden.")]
        public string cConfirmarPassword { get; set; }

        public SettingsViewModel() { }

        public SettingsViewModel(string cUserName)
        {
            Data.dsUsuariosTableAdapters.Operaciones Adapter = new Data.dsUsuariosTableAdapters.Operaciones();
            this.cUserName = cUserName;
            this.cEmail = Adapter.SelectEmail(cUserName).ToString();
        }
    }
}