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
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            
            var ferramentas = _context.Ferramentas.SingleOrDefault(ferr => ferr.Id == id);
            if(ferramentas == null) return NotFound();
            return Ok(ferramentas);
        }
        [HttpPost]
        public IActionResult Add(AddFerramentaModel model)
        {
            try
            {
                bool findFlag = _context.Ferramentas.Any(ferr => ferr.Id == model.Id);
                if (findFlag) return BadRequest("Ferramenta com id já alocado");

                Ferramenta _ferramenta = new(model.Id, model.Nome, model.Descricao);
                _context.Ferramentas.Add(_ferramenta);
                _context.SaveChanges();

                return CreatedAtAction("GetById", new { id = model.Id }, _ferramenta);
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult Delete(int idFerramenta)
        {
            try
            {
                //Validar se a ferramenta encontra-se cadastrada na base de dados
                var ferramenta = _context.Ferramentas.SingleOrDefault(ferr => ferr.Id == idFerramenta);

                if (ferramenta == null) return NotFound();

                _context.Ferramentas.Remove(ferramenta);

                _context.SaveChanges();

                return Ok("deu bom");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
