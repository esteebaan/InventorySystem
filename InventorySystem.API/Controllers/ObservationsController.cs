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
    public class ObservationsController : ControllerBase
    {
        private readonly IObservationService _service;
        private readonly IMapper _mapper;

        public ObservationsController(IObservationService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ Ver observaciones: permitido para Operator y Admin
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpGet("loan/{loanId:guid}")]
        public async Task<IActionResult> GetByLoan(Guid loanId)
        {
            var data = await _service.GetObservationsByLoanAsync(loanId);
            return Ok(_mapper.Map<IEnumerable<ObservationDto>>(data));
        }

        // ✅ Crear observación: permitido para Operator y Admin
        [Authorize(Policy = "OperatorOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateObservationDto dto)
        {
            var entity = _mapper.Map<Observation>(dto);
            var id = await _service.AddObservationAsync(entity);
            return Created("", new { id });
        }
    }
}