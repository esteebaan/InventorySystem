using AutoMapper;
using InventorySystem.API.DTOs;
using InventorySystem.Business.Services;
using InventorySystem.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _service;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ Lectura disponible para operadores y admins
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllArticlesAsync();
            return Ok(_mapper.Map<IEnumerable<ArticleDto>>(data));
        }

        // ✅ Lectura individual también permitida para ambos
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var article = await _service.GetArticleByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(_mapper.Map<ArticleDto>(article));
        }

        // ✅ Crear artículo: permitido para Operator y Admin
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleDto dto)
        {
            var entity = _mapper.Map<Article>(dto);
            var id = await _service.CreateArticleAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        // ✅ Editar artículo: permitido para Operator y Admin
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateArticleDto dto)
        {
            var entity = await _service.GetArticleByIdAsync(id);
            if (entity == null) return NotFound();
            _mapper.Map(dto, entity);
            await _service.UpdateArticleAsync(entity);
            return NoContent();
        }

        // ❌ Eliminar artículo: solo Admin
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}
