using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Ferramentas.Domain.Models
{
    public record UpdateSaidaInventarioModel(int IdFerramenta, uint Matricula)
    {
    }
}
