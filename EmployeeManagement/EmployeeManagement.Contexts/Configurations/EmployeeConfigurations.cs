using EmployeeManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Contexts.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(employee => employee.EmployeeId);

            builder.Property(employee => employee.EmployeeName).HasColumnName("employee_name").HasColumnType("varchar(100)").IsRequired();
            builder.Property(employee => employee.PhoneNumber).HasColumnName("phone_number").HasColumnType("bigint").IsRequired();
            builder.Property(employee => employee.Email).HasColumnName("email").HasColumnType("varchar(100)").IsRequired();
            builder.Property(employee => employee.Salary).HasColumnName("salary").HasColumnType("int").IsRequired();
            builder.Property(employee => employee.Role).HasColumnName("role").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(employee => employee.DepartmentId).HasColumnName("department_id").HasColumnType("int").IsRequired();

            builder.HasOne(employee => employee.Department)
                .WithMany(department => department.Employees)
                .HasForeignKey(employee => employee.DepartmentId);

        }
    }
}
