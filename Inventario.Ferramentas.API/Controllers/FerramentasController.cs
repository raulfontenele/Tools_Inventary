using Microsoft.AspNetCore.Mvc;
using Inventario.Ferramentas.Domain.Entities;
using Inventario.Ferramentas.Domain.Models;
using Inventario.Ferramentas.Infrastructure.Data.Context;

namespace Inventario.Ferramentas.API.Controllers
{
    [Route("api/[controller]")]
    public class FerramentasController : Controller
    {
        private readonly InventarioContext _context;
        public FerramentasController(InventarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var ferramentas = _context.Ferramentas;
            return Ok(ferramentas);
        }
        [HttpPost]
        public IActionResult Put(AddFerramentaModel model)
        {
            try
            {
                bool findFlag = _context.Ferramentas.Any(ferr => ferr.Id == model.Id);
                if (findFlag) return NoContent();

                _context.Ferramentas.Add(
                        new Ferramenta(model.Id, model.Nome, model.Descricao)
                        );
                _context.SaveChanges();

                return Ok("deu bom");
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
