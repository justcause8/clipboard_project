using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clipboard_project.Models
{
    [Table("AccessLevel")]
    public class AccessLevel
    {
        [Key]
        public int ID { get; set; }
        public string LevelAccess { get; set; }
        public DateTime EffectivePeriod { get; set; }
        [ForeignKey("Position_ID")]
        public int Position_ID { get; set; }
    }

    [Table("Department")]
    public class Department
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }

    [Table("Device")]
    public class Device
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string MACAddress { get; set; }
    }

    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    [Table("EmployeeDepartment")]
    public class EmployeeDepartment
    {
        [Key]
        [Column(Order = 0)]
        public int Employee_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int Department_ID { get; set; }
        [ForeignKey("Employee_ID")]
        public Employee Employee { get; set; }
        [ForeignKey("Department_ID")]
        public Department Department { get; set; }
    }

    [Table("EmployeeDevice")]
    public class EmployeeDevice
    {
        [Key]
        [Column(Order = 0)]
        public int Employee_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int Device_ID { get; set; }
        [ForeignKey("Employee_ID")]
        public Employee Employee { get; set; }
        [ForeignKey("Device_ID")]
        public Device Device { get; set; }
    }

    [Table("ExchangeHistory")]
    public class ExchangeHistory
    {
        [Key]
        public int ID { get; set; }
        public DateTime ExchangeDate { get; set; }
        public string ExchangeStatus { get; set; }
        [ForeignKey("Employee_ID")]
        public Employee Employee { get; set; }
        [ForeignKey("Device_ID")]
        public Device Device { get; set; }
    }

    [Table("FileExchangeHistory")]
    public class FileExchangeHistory
    {
        [Key]
        [Column(Order = 0)]
        public int File_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ExchangeHistory_ID { get; set; }
        [ForeignKey("File_ID")]
        public FileMain FileMain { get; set; }
        [ForeignKey("ExchangeHistory_ID")]
        public ExchangeHistory ExchangeHistory { get; set; }
    }

    [Table("FileMain")]
    public class FileMain
    {
        public int ID { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }

    }


    [Table("FileMainLocation")]
    public class FileMainLocation
    {
        [Key]
        public int ID { get; set; }
        public string AbsolutePath { get; set; }
        [ForeignKey("File_ID")]
        public FileMain FileMain { get; set; }
    }

    [Table("Position")]
    public class Position
    {
        [Key]
        public int ID { get; set; }
        public string NewPosition { get; set; }
        public DateTime DateReceived { get; set; }

        [ForeignKey("Employee_ID")]
        public int Employee_ID { get; set; }
    }

    public class CloudDBContext : DbContext
    {
        public CloudDBContext(DbContextOptions<CloudDBContext> options) : base(options)
        {
        }

        public DbSet<AccessLevel> AccessLevels { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<EmployeeDevice> EmployeeDevices { get; set; }
        public DbSet<ExchangeHistory> ExchangeHistories { get; set; }
        public DbSet<FileExchangeHistory> FileExchangeHistories { get; set; }
        public DbSet<FileMain> FileMains { get; set; }
        public DbSet<FileMainLocation> FileMainLocations { get; set; }
        public DbSet<Position> Positions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLevel>()
                .HasKey(a => a.ID);
            modelBuilder.Entity<ExchangeHistory>()
                .HasKey(e => e.ID);
            modelBuilder.Entity<Device>()
                .HasKey(d => d.ID);
            modelBuilder.Entity<FileMain>()
                .HasKey(f => f.ID);
            modelBuilder.Entity<FileMainLocation>()
                .HasKey(f => f.ID);
            modelBuilder.Entity<Department>()
                .HasKey(d => d.ID);
            modelBuilder.Entity<Employee>()
                .HasKey(d => d.ID);
            modelBuilder.Entity<EmployeeDepartment>()
                .HasKey(ed => new { ed.Employee_ID, ed.Department_ID });
            modelBuilder.Entity<EmployeeDevice>()
                .HasKey(ed => new { ed.Employee_ID, ed.Device_ID });
            modelBuilder.Entity<FileExchangeHistory>()
                .HasKey(fh => new { fh.File_ID, fh.ExchangeHistory_ID });
            modelBuilder.Entity<Position>()
                .HasKey(p => new { p.ID });

            base.OnModelCreating(modelBuilder);
        }
    }
}
