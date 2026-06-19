using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Infrastructure.DataAccess.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
           builder.HasKey(j => j.JobId);
            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(j => j.Location)
                .HasMaxLength(200);
            builder.Property(j => j.Salary)
                .HasColumnType("decimal(18,2)");
            builder.Property(j => j.JobType)
                .HasMaxLength(50);

            builder.HasOne(j => j.Employer)
                .WithMany(e => e.Jobs)
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
