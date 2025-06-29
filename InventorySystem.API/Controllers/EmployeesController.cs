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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ Solo Admin puede listar empleados
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllEmployeesAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(data));
        }

        // ✅ Solo Admin puede ver un empleado
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee = await _service.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        // ✅ Solo Admin puede crear empleados
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var entity = _mapper.Map<Employee>(dto);
            var id = await _service.CreateEmployeeAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        // ✅ Solo Admin puede actualizar empleados
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeDto dto)
        {
            var entity = await _service.GetEmployeeByIdAsync(id);
            if (entity == null) return NotFound();
            _mapper.Map(dto, entity);
            await _service.UpdateEmployeeAsync(entity);
            return NoContent();
        }

        // ✅ Solo Admin puede eliminar empleados
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}