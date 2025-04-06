using EmployeeManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Contexts.Configurations
{
    public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");
            builder.HasKey(department => department.DepartmentId);

            builder.Property(department => department.DepartmentName)
                .HasColumnName("department_name")
                .HasColumnType("varchar(100)")
                .IsRequired();
        }
    }
}
