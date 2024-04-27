using Microsoft.AspNetCore.Mvc;
using clipboard_project.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace clipboard_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private List<Employee> employees = new List<Employee>
        {
            new Employee(1, "Иван", "Иванов", "Иванович", "Программист", new DateTime(1990, 5, 15)),
            new Employee(2, "Петр", "Петров", "Петрович", "Менеджер", new DateTime(1985, 10, 20))
        };

        // GET: api/employees
        [HttpGet]
        public IActionResult GetEmployees()
        {
            // Возвращаем всех сотрудников
            return Ok(employees);
        }

        // GET: /api/Employees/{id}
        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            // Находим сотрудника по Id
            var employee = employees.FirstOrDefault(emp => emp.Id == id);

            if (employee == null)
            {
                // Если сотрудник с указанным Id не найден, возвращаем ошибку 404
                return NotFound($"Employee with id {id} not found");
            }

            // Возвращаем информацию о сотруднике
            return Ok(employee);
        }

        // POST: api/employees
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee object is null");
            }

            // Генерируем новый Id для сотрудника
            int newId = employees.Max(emp => emp.Id) + 1;
            employee.Id = newId;

            // Добавляем сотрудника в коллекцию
            employees.Add(employee);

            // Возвращаем созданный сотрудник с его новым Id
            return CreatedAtAction(nameof(GetEmployee), new { id = newId }, employee);
        }

    // PUT api/<EmployeeController>/5
    [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
