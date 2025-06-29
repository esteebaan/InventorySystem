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
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;
        private readonly IMapper _mapper;

        public LoansController(ILoanService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ Lectura para Operator y Admin
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllLoansAsync();
            return Ok(_mapper.Map<IEnumerable<LoanDto>>(data));
        }

        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var loan = await _service.GetLoanByIdAsync(id);
            if (loan == null) return NotFound();
            return Ok(_mapper.Map<LoanDto>(loan));
        }

        // ✅ Crear préstamo: permitido para Operator y Admin
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLoanDto dto)
        {
            var entity = _mapper.Map<Loan>(dto);
            var id = await _service.CreateLoanAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        // ✅ Actualizar préstamo (por ejemplo, marcar como devuelto)
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateLoanDto dto)
        {
            var entity = await _service.GetLoanByIdAsync(id);
            if (entity == null) return NotFound();
            _mapper.Map(dto, entity);
            await _service.UpdateLoanAsync(entity);
            return NoContent();
        }

        // ❌ Solo Admin puede eliminar un préstamo
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteLoanAsync(id);
            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            try
            {
                await _service.UpdateLoanStatusAsync(id, status);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}