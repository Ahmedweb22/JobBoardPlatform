using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Infrastructure.DataAccess.Configurations
{
    internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(a => a.ApplicationId);

            builder.Property(a => a.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            builder.HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Candidate)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
