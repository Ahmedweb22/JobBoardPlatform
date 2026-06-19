using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Infrastructure.DataAccess.Configurations
{
    public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
    {
        public  void Configure(EntityTypeBuilder<Employer> builder)
        {
            builder.HasKey(e => e.EmployerId);
            builder.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.CompanyDescription)
                .HasMaxLength(1000);
            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Employer>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
