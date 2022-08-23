﻿using System.ComponentModel.DataAnnotations;

namespace TrigonosEnergyWebAPI.DTO
{
    public class UsuarioAuthDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string? Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(10,MinimumLength =4,ErrorMessage ="La contraseña debe estar entre 4 y 10 caracteres")]
        public string? Password { get; set; }
    }
}