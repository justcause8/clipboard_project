using Microsoft.AspNetCore.Mvc;
using clipboard_project.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace clipboard_project.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Employee>>> GetFileMains()
        {
            return await _context.Employees.ToListAsync();
        }

        // POST: api/employee
        [HttpPost]
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
    public class FileMainController : ControllerBase
    {
        private readonly CloudDBContext _context;

        public FileMainController(CloudDBContext context)
        {
            _context = context;
        }

        // GET: api/filemain
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileMain>>> GetFileMains()
        {
            return await _context.FileMains.ToListAsync();
        }

        // POST: api/filemain
        [HttpPost]
        public async Task<ActionResult<FileMain>> PostFileMain(FileMain fileMain)
        {
            _context.FileMains.Add(fileMain);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFileMains), new { id = fileMain.ID }, fileMain);
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
