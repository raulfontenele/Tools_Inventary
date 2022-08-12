using Microsoft.AspNetCore.Mvc;
using Inventario.Ferramentas.Domain.Models;
using Inventario.Ferramentas.Domain.Entities;
using Inventario.Ferramentas.Domain.Extensions;
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

        #region Inventario Padrão
        [HttpGet]
        public IActionResult GetAll()
        {
            var inventarioCompleto = _context.InventarioFerramentas;
            return Ok(inventarioCompleto);
        }
        [HttpGet("{idFerramenta}")]
        public IActionResult GetById(int idFerramenta)
        {
            var inventario = _context.InventarioFerramentas.SingleOrDefault(inv => inv.IdFerramenta == idFerramenta);
            if (inventario == null) return NotFound();
            return Ok(inventario);
        }
        [HttpPost]
        public IActionResult Add(int idFerramenta)
        {
            try
            {
                //Validar se a ferramenta já se encontra cadastrada no sistema
                if (!_context.Ferramentas.Any(ferr => ferr.Id == idFerramenta)) return NotFound();

                if (_context.InventarioFerramentas.Any(ferr => ferr.IdFerramenta == idFerramenta)) return BadRequest("Ferramenta já alocada");

                InventarioFerramentas _inventarioFerramentas = new(idFerramenta);

                _context.InventarioFerramentas.Add(_inventarioFerramentas);

                _context.SaveChanges();

                return CreatedAtAction("GetById", new { idFerramenta = idFerramenta }, _inventarioFerramentas);
            }
            catch (Exception ex)
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
                var ferramenta = _context.InventarioFerramentas.SingleOrDefault(ferr => ferr.IdFerramenta == idFerramenta);

                if (ferramenta == null) return NotFound();

                _context.InventarioFerramentas.Remove(ferramenta);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        #endregion

        #region Devoulucao
        [Route("api/[controller]/Devolucao")]
        [HttpPut]
        public IActionResult UpdateToolReturn(int idFerramenta)
        {
            var ferramenta = _context.InventarioFerramentas.SingleOrDefault(inv => inv.IdFerramenta == idFerramenta);
            if (ferramenta == null) return NotFound();

            if (ferramenta.Status == Status.Estoque) return BadRequest("Ferramenta não está emprestada");

            ferramenta.Status = Status.Estoque;
            ferramenta.DataDevolucao = DateTime.Now;

            _context.SaveChanges();

            return NoContent();
        }
        #endregion

        #region Emprestimo
        [Route("api/[controller]/Emprestimo")]
        [HttpGet]
        public IActionResult GetAllEmprestadas()
        {
            var inventarioFiltro = _context.InventarioFerramentas.Where(ferr => ferr.Status == Status.Emprestado);
            return Ok(inventarioFiltro);
        }
        [Route("api/[controller]/Emprestimo")]
        [HttpPut]
        public IActionResult UpdateToolOutput(UpdateSaidaInventarioModel model)
        {
            try
            {
                var ferramenta = _context.InventarioFerramentas.SingleOrDefault(inv => inv.IdFerramenta == model.IdFerramenta);
                if (ferramenta == null) return NotFound();

                if (ferramenta.Status == Status.Emprestado) return BadRequest("Ferramenta não disponível");

                ferramenta.Status = Status.Emprestado;
                ferramenta.MatriculaUsuario = model.Matricula;
                ferramenta.DataEmprestimo = DateTime.Now;

                if (ferramenta.DataDevolucao.HasValue) ferramenta.DataDevolucao = null;

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }

        }
        [Route("api/[controller]/Emprestimo/{matricula}")]
        [HttpGet]
        public IActionResult GetByUser(uint matricula)
        {
            var inventarioFiltro = _context.InventarioFerramentas.Where(ferr => ferr.MatriculaUsuario == matricula);
            return Ok(inventarioFiltro);
        }
        #endregion
    }
}
