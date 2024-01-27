using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
          
      builder
      .HasOne(m => m.Request)
      .WithMany(r => r.Messages)
      .HasForeignKey(r => r.RequestId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

    

    }

}