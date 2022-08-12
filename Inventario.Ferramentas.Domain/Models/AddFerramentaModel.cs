using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Ferramentas.Domain.Models
{
    public record AddFerramentaModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public string? Descricao { get; set; }
    }
}
