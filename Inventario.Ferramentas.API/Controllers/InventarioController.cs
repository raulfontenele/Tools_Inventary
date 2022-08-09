﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpPatch]
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

                return Ok("Deu bom");
            }
            catch (Exception)
            {
                return NotFound();
            }
            
        }
        [HttpPatch("{idFerramenta}")]
        public IActionResult UpdateToolReturn(int idFerramenta)
        {
            var ferramenta = _context.InventarioFerramentas.SingleOrDefault(inv => inv.IdFerramenta == idFerramenta);
            if (ferramenta == null) return NotFound();

            if (ferramenta.Status == Status.Estoque) return BadRequest("Ferramenta não está emprestada");

            ferramenta.Status = Status.Estoque;
            ferramenta.DataDevolucao = DateTime.Now;

            _context.SaveChanges();

            return Ok("Deu bom");
        }
        [HttpPost]
        public IActionResult Add(int idFerramenta)
        {
            try
            {
                //Validar se a ferramenta já se encontra cadastrada no sistema
                if (!_context.Ferramentas.Any(ferr => ferr.Id == idFerramenta)) return NotFound();

                if(_context.InventarioFerramentas.Any(ferr => ferr.IdFerramenta == idFerramenta)) return BadRequest("Ferramenta já alocada");

                _context.InventarioFerramentas.Add(
                    new InventarioFerramentas(idFerramenta)
                    );

                _context.SaveChanges();

                return Ok("deu bom");
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
        [Route("api/[controller]/Emprestimo")]
        [HttpGet]
        public IActionResult GetAllEmprestadas()
        {
            var inventarioFiltro = _context.InventarioFerramentas.Where(ferr => ferr.Status == Status.Emprestado);
            return Ok(inventarioFiltro);
        }
    }
}
