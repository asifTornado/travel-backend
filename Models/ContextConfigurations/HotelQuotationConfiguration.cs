using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace backEnd.Models.ContextConfigurations;

public class HotelQuotationConfiguration : IEntityTypeConfiguration<HotelQuotation>
{
    public void Configure(EntityTypeBuilder<HotelQuotation> builder)
    {
        builder.HasOne(q => q.Request)
        .WithMany(r => r.HotelQuotations)
        .HasForeignKey(q => q.RequestId)
        .OnDelete(DeleteBehavior.Cascade);


        
      builder.Property(x => x.RequestIds)
      .HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());

    builder.HasMany(x => x.HotelApprovals)
      .WithMany(x => x.HotelApproved)
      .UsingEntity<HotelApprovals>(
           x => x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId),
           y => y.HasOne(y=> y.Quotation).WithMany().HasForeignKey(x => x.QuotationId)
      );

      builder.HasMany(x => x.Invoices)
      .WithMany(x => x.Quotations)
      .UsingEntity<HotelQuotationInvoices>(
             x => x.HasOne(x => x.Invoice).WithMany().HasForeignKey(x => x.InvoiceId),
             y => y.HasOne(y => y.Quotation).WithMany().HasForeignKey(x => x.QuotationId)
      );


      builder.Property(x => x.TotalCosts)
      .HasConversion(
        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
        v => JsonSerializer.Deserialize<List<TravelerCost>>(v, JsonSerializerOptions.Default)

      );

      
    }


}