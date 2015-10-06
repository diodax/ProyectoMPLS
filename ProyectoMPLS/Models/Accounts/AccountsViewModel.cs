using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
}