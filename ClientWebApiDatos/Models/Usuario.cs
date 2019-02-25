using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientWebApiDatos.Models
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Usuario")]
        public string usuario_red { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Estado")]
        public string estado { get; set; }
    }
}