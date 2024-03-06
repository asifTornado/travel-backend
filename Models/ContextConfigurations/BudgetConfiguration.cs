using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace backEnd.Models.ContextConfigurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {          

             
     
    
      
             builder.HasMany(x => x.Travelers)
             .WithMany(x => x.Budgets)
             .UsingEntity(j => j.ToTable("BudgetTravelers"));


             builder
            .Property(b => b.TripId)
            .HasComputedColumnSql("CONCAT('B', RIGHT('00000' + CAST(Id AS NVARCHAR(5)), 5))")
            .ValueGeneratedOnAddOrUpdate();

            
    builder.Property(x => x.PrevHandlerIds)
    .HasColumnType("nvarchar(max)")
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<int>>(v, JsonSerializerOptions.Default)
    );

    builder.Property(x => x.AuditPrevHandlerIds)
    .HasColumnType("nvarchar(max)")
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<int>>(v, JsonSerializerOptions.Default)
    );

    builder.Property(x => x.AccountsPrevHandlerIds)
    .HasColumnType("nvarchar(max)")
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<int>>(v, JsonSerializerOptions.Default)
    );


         builder
        .HasMany(e => e.TicketApprovals)
        .WithMany(e => e.BudgetTicketsApproved)
        .UsingEntity<BudgetTicketApprovals>(
           l => l.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId),
           z => z.HasOne(x => x.Budget).WithMany().HasForeignKey(x=> x.BudgetId)
        );

    }
    
    
}
