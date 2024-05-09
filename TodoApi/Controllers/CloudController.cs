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
                // Return server error with error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return StatusCode(500, new { errors = errors });
            }

            var now = DateTime.UtcNow;

            // Generate a symmetric security key with a key size of 256 bits
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.KEY));

            // Create JWT token using the updated symmetric security key
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
            // Ищем пользователя в базе данных по имени пользователя и паролю
            var person = _context.Employees.SingleOrDefault(x => x.Username == username && x.Password == password);

            // Проверяем, найден ли пользователь
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Username),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // Если пользователь не найден, возвращаем null
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
        [CustomAuthorization("Lenka")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetFileMains()
        {
            return await _context.Employees.ToListAsync();
        }

        // POST: api/employee
        [HttpPost]
        [CustomAuthorization("Dmitr")]
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
        public async Task<IActionResult> GetFileMain(int id)
        {
            var fileMain = await _context.FileMains.FindAsync(id);

            if (fileMain == null)
            {
                return NotFound();
            }

            try
            {
                // Возврат байтов файла из базы данных в качестве файла для скачивания
                return File(fileMain.Data, GetContentType(fileMain.Extension), fileMain.Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }


        private string GetContentType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".pdf":
                    return "application/pdf";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".txt":
                    return "text/plain";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                default:
                    return "application/octet-stream";
            }
        }


        [HttpPost("{id}")]
        public async Task<ActionResult<FileMain>> UploadFile(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл пуст или его нет.");
            }

            try
            {
                // Определяем пороговый размер для сохранения в БД
                long dbThreshold = 1024 * 1024;

                // Если размер файла превышает пороговый, сохраняем на сервере
                if (file.Length > dbThreshold)
                {
                    // Чтение байтов файла и сохранение на сервере
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine("uploads", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Создание объекта FileMain для сохранения в базе данных
                    var fileMain = new FileMain
                    {
                        ID = id + 600,
                        Data = null,
                        Name = fileName,
                        Size = file.Length,
                        Extension = Path.GetExtension(file.FileName)
                    };

                    // Добавление объекта FileMain в контекст и сохранение изменений в базе данных
                    _context.FileMains.Add(fileMain);
                    await _context.SaveChangesAsync();

                    // Возврат созданного объекта FileMain
                    return CreatedAtAction(nameof(UploadFile), new { id = fileMain.ID }, fileMain);
                }
                else // Иначе сохраняем файл в столбце в базе данных
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();

                        // Создание объекта FileMain для сохранения в базе данных
                        var fileMain = new FileMain
                        {
                            ID = id + 600,
                            Data = fileBytes,
                            Name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                            Size = file.Length,
                            Extension = Path.GetExtension(file.FileName)
                        };

                        // Добавление объекта FileMain в контекст и сохранение изменений в базе данных
                        _context.FileMains.Add(fileMain);
                        await _context.SaveChangesAsync();

                        // Возврат созданного объекта FileMain
                        return CreatedAtAction(nameof(UploadFile), new { id = fileMain.ID }, fileMain);
                    }
                }
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}. Внутреннее исключение: {innerExceptionMessage}");
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
}