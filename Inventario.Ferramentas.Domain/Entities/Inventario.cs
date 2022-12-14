using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario.Ferramentas.Domain.Extensions;

namespace Inventario.Ferramentas.Domain.Entities
{
    public class InventarioFerramentas
    {
        public InventarioFerramentas(int idFerramenta)
        {
            IdFerramenta = idFerramenta;
            Status = Status.Estoque;
        }

        public int IdFerramenta { get; set; }
        public Status Status { get; set; }
        public uint? MatriculaUsuario { get; set; }
        public DateTime? DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
    }
}
