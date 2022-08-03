using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Ferramentas.Domain.Extensions;

namespace Inventario.Ferramentas.Domain.Entities
{
    public class Ferramenta
    {
        public Ferramenta(int id, string? nome, string? descricao)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
        }

        public int Id { get; private set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
    }
}
