using Microsoft.AspNetCore.Mvc;
using Inventario.Ferramentas.Domain.Entities;
using Inventario.Ferramentas.Domain.Models;
using Inventario.Ferramentas.Infrastructure.Data.Context;

namespace Inventario.Ferramentas.API.Controllers
{
    [Route("api/[controller]")]
    public class InventarioController : Controller
    {
        private readonly InventarioContext _context;
        public InventarioController(InventarioContext context)
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
                //Validar se a ferramenta já se encontra cadastrada no sistema
                //using (var context = new InventarioContext())
                //{
                    bool findFlag = _context.Ferramentas.Any(ferr => ferr.Id == model.Id);
                    if (findFlag) NoContent();

                _context.Ferramentas.Add(
                        new Ferramenta(model.Id, model.Nome, model.Descricao)
                        );
                _context.SaveChanges();

                //}
                return Ok("deu bom");
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
