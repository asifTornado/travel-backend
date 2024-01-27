using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace backEnd.Models.ContextConfigurations;

public class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
      builder.HasOne(q => q.Request)
      .WithMany(r => r.Quotations)
      .HasForeignKey(q => q.RequestId)
      .OnDelete(DeleteBehavior.Cascade);

      builder.Property(x => x.RequestIds)
      .HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());


      builder.Property(x => x.TotalCosts)
      .HasConversion(
        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
        v => JsonSerializer.Deserialize<List<TravelerCost>>(v, JsonSerializerOptions.Default)

      );


      builder.HasMany(x => x.TicketApprovals)
      .WithMany(x => x.TicketApproved)
      .UsingEntity( x => x.ToTable("TicketApprovals"));

      builder.HasMany(x => x.Invoices)
      .WithMany(x => x.Quotations)
      .UsingEntity( x => x.ToTable("TicketInvoiceQuotations"));
      

    }


}