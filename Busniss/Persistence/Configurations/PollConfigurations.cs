using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Busniss.Persistence.Configurations;
public class PollConfigurations : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.Property(p => p.Title).IsRequired();
        builder.HasIndex(p => p.Title).IsUnique();
    }
}
