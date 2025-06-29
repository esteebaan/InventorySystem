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
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;
        private readonly IMapper _mapper;

        public ClientsController(IClientService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ Lectura permitida para ambos roles
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllClientsAsync();
            return Ok(_mapper.Map<IEnumerable<ClientDto>>(data));
        }

        // ✅ Lectura individual permitida para ambos
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var client = await _service.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(_mapper.Map<ClientDto>(client));
        }

        // ✅ Crear cliente: Admin y Operator
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateClientDto dto)
        {
            var entity = _mapper.Map<Client>(dto);
            var id = await _service.CreateClientAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        // ✅ Actualizar cliente: Admin y Operator
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateClientDto dto)
        {
            var entity = await _service.GetClientByIdAsync(id);
            if (entity == null) return NotFound();
            _mapper.Map(dto, entity);
            await _service.UpdateClientAsync(entity);
            return NoContent();
        }

        // ❌ Eliminar cliente: solo Admin
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteClientAsync(id);
            return NoContent();
        }
    }
}