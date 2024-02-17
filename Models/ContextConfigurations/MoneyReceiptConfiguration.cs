using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;


namespace backEnd.Models.ContextConfigurations;

public class MoneyReceiptConfiguration : IEntityTypeConfiguration<MoneyReceipt>
{
    public void Configure(EntityTypeBuilder<MoneyReceipt> builder)
    {
       
             
      builder.Property(x => x.Approvals)
      .HasConversion(
        x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
        x => JsonSerializer.Deserialize<List<User>>(x, JsonSerializerOptions.Default)
      );


      builder.Property(x => x.PrevHandlerIds)
    .HasColumnType("nvarchar(max)")
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<int>>(v, JsonSerializerOptions.Default)
    );

      builder
      .HasOne(r => r.CurrentHandler)
      .WithMany(u => u.CurrentReceiptsHandled)
      .HasForeignKey(u => u.CurrentHandlerId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

      builder
      .HasOne(x => x.Request)
      .WithOne(x => x.MoneyReceipt)
      .HasForeignKey<MoneyReceipt>(x => x.RequestId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

    }
}