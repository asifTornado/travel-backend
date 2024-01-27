using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.HasOne(l => l.Request).WithMany(r => r.Logs).HasForeignKey(l => l.RequestId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
    }
}