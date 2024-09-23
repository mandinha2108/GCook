using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GCook.ViewModels
{
    public class RegistroVM
    {
      [Display (Name = "Nome Completo", Prompt = "Informe seu Nome Completo")]
      [Required(ErrorMessage = "Por favor, informe seu Nome")]
      [StringLength(60, ErrorMessage = "O Nome deve possuir no máximo 60 caracteres")]
      public string Nome { get; set; }


      [DataType(DataType.Date)]
      [Display(Name = "Data de Nascimento", Prompt = "Informe sua Data de Nascimento")]
      [Required(ErrorMessage = "Por favor, informe sua Data de Nascimento")]
      public DateTime? DataNascimento { get; set; } = null;

      [Display(Prompt = "Informe seu Email")]
      []
    }
}