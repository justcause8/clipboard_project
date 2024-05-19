using Microsoft.AspNetCore.Mvc;
using clipboard_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace clipboard_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly CloudDBContext _context;

        public AccountController(CloudDBContext context)
        {
            _context = context;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest("Invalid username or password.");
            }

            var now = DateTime.UtcNow;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.KEY));

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person = _context.Employees.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Username),
                    new Claim(AuthOptions.UserIdClaimType, person.ID.ToString())  // Добавляем ID пользователя в токен
                };

                return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }

            return null;
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AccessLevelController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public AccessLevelController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/accesslevel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessLevel>>> GetAccessLevels()
        {
            return await _context.AccessLevels.ToListAsync();
        }

        // POST: api/accesslevel
        [HttpPost]
        public async Task<ActionResult<AccessLevel>> PostAccessLevel(AccessLevel accessLevel)
        {
            _context.AccessLevels.Add(accessLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccessLevels), new { id = accessLevel.ID }, accessLevel);
        }
    }




    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public DepartmentController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetFileMains()
        {
            return await _context.Departments.ToListAsync();
        }

        // POST: api/department
        [HttpPost]
        public async Task<ActionResult<Department>> PostFileMain(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { id = department.ID }, department);
        }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public DeviceController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/device
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // POST: api/device
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDevices), new { id = device.ID }, device);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public EmployeeController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/employee
        [HttpGet]
        [CustomAuthorization]
        public async Task<ActionResult<IEnumerable<Employee>>> GetFileMains()
        {
            return await _context.Employees.ToListAsync();
        }

        // POST: api/employee
        [HttpPost]
        [CustomAuthorization]
        public async Task<ActionResult<Employee>> PostFileMain(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { id = employee.ID }, employee);
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDepartmentController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public EmployeeDepartmentController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/employee
        [HttpGet]

        public async Task<ActionResult<IEnumerable<EmployeeDepartment>>> GetFileMains()
        {
            return await _context.EmployeeDepartments.ToListAsync();
        }

        // POST: api/employee
        [HttpPost]
        public async Task<ActionResult<EmployeeDepartment>> PostFileMain(EmployeeDepartment employeeDepartment)
        {
            _context.EmployeeDepartments.Add(employeeDepartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { Employee_ID = employeeDepartment.Employee_ID, Department_ID = employeeDepartment.Department_ID }, employeeDepartment);
        }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDeviceController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public EmployeeDeviceController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/employeeDevice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDevice>>> GetFileMains()
        {
            return await _context.EmployeeDevices.ToListAsync();
        }

        // POST: api/employeeDevice
        [HttpPost]
        public async Task<ActionResult<EmployeeDevice>> PostFileMain(EmployeeDevice employeeDevice)
        {
            _context.EmployeeDevices.Add(employeeDevice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { Employee_ID = employeeDevice.Employee_ID, Device_ID = employeeDevice.Device_ID }, employeeDevice);
        }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeHistoryController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public ExchangeHistoryController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/exchangehistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeHistory>>> GetExchangeHistories()
        {
            return await _context.ExchangeHistories.ToListAsync();
        }

        // POST: api/exchangehistory
        [HttpPost]
        public async Task<ActionResult<ExchangeHistory>> PostExchangeHistory(ExchangeHistory exchangeHistory)
        {
            _context.ExchangeHistories.Add(exchangeHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExchangeHistories), new { id = exchangeHistory.ID }, exchangeHistory);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("current")]
        [CustomAuthorization] // Заменяем атрибут авторизации
        public IActionResult GetCurrentUserId()
        {
            // Получаем ID пользователя из атрибута авторизации
            var userId = HttpContext.Items["userId"] as string;

            if (userId == null)
            {
                // Если ID пользователя не найден, возвращаем ошибку
                return NotFound("User ID not found.");
            }

            // Возвращаем ID текущего пользователя
            return Ok(userId);
        }
    }


    //Работа с файлами
    [Route("api/[controller]")]
    [ApiController]
    public class FileMainController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public FileMainController(CloudDBContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [CustomAuthorization]
        public async Task<IActionResult> GetFileMain(int id)
        {
            var userId = GetUserIdFromToken();
            var fileMain = await _context.FileMains.FirstOrDefaultAsync(fm => fm.ID == id && fm.Employee_ID == userId);

            if (fileMain == null)
            {
                return NotFound();
            }

            try
            {
                return File(fileMain.Data, GetContentType(fileMain.Extension), fileMain.Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GetContentType(string fileExtension)
        {
            return fileExtension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                _ => "application/octet-stream",
            };
        }

        [HttpPost("{fileId}")]
        [CustomAuthorization]
        public async Task<ActionResult<FileMain>> UploadFile(int fileId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            try
            {
                var userId = int.Parse(HttpContext.Items["userId"] as string);

                long dbThreshold = 1024 * 1024;

                if (file.Length > dbThreshold)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine("uploads", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileMain = new FileMain
                    {
                        ID = fileId, // Используем ID файла из URL
                        Name = fileName,
                        Size = file.Length,
                        Extension = Path.GetExtension(file.FileName),
                        Employee_ID = userId,
                        Data = null,
                    };

                    _context.FileMains.Add(fileMain);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetFileMain), new { id = fileMain.ID }, fileMain);
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();

                        var fileMain = new FileMain
                        {
                            ID = fileId, // Используем ID файла из URL
                            Employee_ID = userId,
                            Data = fileBytes,
                            Name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                            Size = file.Length,
                            Extension = Path.GetExtension(file.FileName)
                        };

                        _context.FileMains.Add(fileMain);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction(nameof(GetFileMain), new { id = fileMain.ID }, fileMain);
                    }
                }
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                return StatusCode(500, $"Internal server error: {ex.Message}. Inner exception: {innerExceptionMessage}");
            }
        }


        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == AuthOptions.UserIdClaimType);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class FileMainLocationController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public FileMainLocationController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/fileMainLocation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileMainLocation>>> GetFileMains()
        {
            return await _context.FileMainLocations.ToListAsync();
        }

        // POST: api/fileMainLocation
        [HttpPost]
        public async Task<ActionResult<FileMainLocation>> PostFileMain(FileMainLocation fileMainLocation)
        {
            _context.FileMainLocations.Add(fileMainLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { id = fileMainLocation.ID }, fileMainLocation);
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class FileExchangeHistoryController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public FileExchangeHistoryController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/fileExchangeHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileExchangeHistory>>> GetFileMains()
        {
            return await _context.FileExchangeHistories.ToListAsync();
        }

        // POST: api/fileExchangeHistory
        [HttpPost]
        public async Task<ActionResult<FileExchangeHistory>> PostFileMain(FileExchangeHistory fileExchangeHistory)
        {
            _context.FileExchangeHistories.Add(fileExchangeHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { File_ID = fileExchangeHistory.File_ID, ExchangeHistory_ID = fileExchangeHistory.ExchangeHistory_ID }, fileExchangeHistory);
        }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public PositionController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/position
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetFileMains()
        {
            return await _context.Positions.ToListAsync();
        }

        // POST: api/position
        [HttpPost]
        public async Task<ActionResult<Position>> PostFileMain(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { id = position.ID }, position);
        }
    }
}