﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hachodromo.Shared.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)] // Puedes ajustar esto dependiendo de tus requisitos de seguridad
        public string Password { get; set; } = string.Empty;
    }

}