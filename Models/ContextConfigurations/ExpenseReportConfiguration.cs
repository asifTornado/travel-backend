using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace backEnd.Models.ContextConfigurations;

public class ExpenseReportConfiguration : IEntityTypeConfiguration<ExpenseReport>
{
    public void Configure(EntityTypeBuilder<ExpenseReport> builder)
    {

        
    builder.HasMany(x => x.Expenses)
    .WithOne(x => x.ExpenseReport)
    .HasForeignKey(x => x.ExpenseReportId)
    .OnDelete(DeleteBehavior.Cascade);

     builder.Property(x => x.PrevHandlerIds)
    
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<int>>(v, JsonSerializerOptions.Default)
    );


     builder.Property(x => x.Approvals)
      .HasConversion(
        x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
        x => JsonSerializer.Deserialize<List<User>>(x, JsonSerializerOptions.Default)
      );




        builder
      .HasOne(r => r.CurrentHandler)
      .WithMany(u => u.CurrentExpenseReportsHandled)
      .HasForeignKey(u => u.CurrentHandlerId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);
    
    }



}