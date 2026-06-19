using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Infrastructure.DataAccess.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public  void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasKey(c => c.CandidateId);
            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(20);
            builder.Property(c => c.CVPath)
                .HasMaxLength(500);
            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Candidate>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
