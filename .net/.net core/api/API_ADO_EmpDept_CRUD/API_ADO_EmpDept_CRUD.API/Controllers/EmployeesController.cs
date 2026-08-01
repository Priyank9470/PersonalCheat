using API_ADO_EmpDept_CRUD.Models.DTOs;
using API_ADO_EmpDept_CRUD.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_ADO_EmpDept_CRUD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllEmployeesAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateUpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.CreateEmployeeAsync(dto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.EmployeeId }, result.Data);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.UpdateEmployeeAsync(id, dto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            }

            return Ok(new { message = $"Employee with ID {id} deleted successfully." });
        }
    }
}
