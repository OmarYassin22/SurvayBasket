﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Busniss.Persistence.Configurations;
public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique();
    }


}
