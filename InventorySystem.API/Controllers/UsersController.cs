using AutoMapper;
using InventorySystem.API.DTOs;
using InventorySystem.Business.Services;
using InventorySystem.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace InventorySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UsersController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ✅ Solo Admin puede ver todos los usuarios
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllUsersAsync();
            var dtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(dtos);
        }

        // ✅ Solo Admin puede ver un usuario
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user is null) return NotFound();
            return Ok(_mapper.Map<UserDTO>(user));
        }

        // ✅ Solo Admin puede crear usuarios
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            // Validar email duplicado
            try
            {
                var hashed = HashPassword(dto.Password);
                var id = await _service.CreateUserWithRoleAsync(dto.FirstName, dto.LastName, dto.Email, hashed, dto.Role);
                return CreatedAtAction(nameof(GetById), new { id }, null);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Solo Admin puede actualizar usuarios
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                await _service.UpdateUserAsync(id, dto.FirstName, dto.LastName, dto.Email, dto.Role);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

            // ✅ Solo Admin puede eliminar usuarios
            [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteUserAsync(id);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            try
            {
                var hashed = Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password));
                var role = "Operator"; // Siempre operador por defecto

                var id = await _service.CreateUserWithRoleAsync(dto.FirstName, dto.LastName, dto.Email, hashed, role);
                return Ok(new { message = "User registered successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        private string HashPassword(string password)
        {
            // 🔐 Mejor usar un algoritmo seguro (esto es temporal)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

    }
}