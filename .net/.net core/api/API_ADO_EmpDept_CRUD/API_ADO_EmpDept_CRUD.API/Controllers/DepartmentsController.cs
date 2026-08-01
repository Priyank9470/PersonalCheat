using API_ADO_EmpDept_CRUD.Models.DTOs;
using API_ADO_EmpDept_CRUD.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_ADO_EmpDept_CRUD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateUpdateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentService.CreateDepartmentAsync(dto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.DepartmentId }, result.Data);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentService.UpdateDepartmentAsync(id, dto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }

            return Ok(new { message = $"Department with ID {id} deleted successfully." });
        }
    }
}
