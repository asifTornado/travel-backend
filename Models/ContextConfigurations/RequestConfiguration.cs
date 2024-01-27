using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;
using System.Text.Json;


namespace backEnd.Models.ContextConfigurations;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        // Fluent API configuration for the "Product" entity
        // Add other configurations as needed
      builder
      .Property( e => e.Number)
      .HasComputedColumnSql("REPLICATE('0', 7 - LEN(Id)) + CAST(Id AS VARCHAR(7))");  


       builder
      .HasMany(r => r.Costs)
      .WithOne(c => c.Request)
      .HasForeignKey(r => r.RequestId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

      builder
      .HasOne( r => r.ExpenseReport)
      .WithOne( r => r.Request)
      .HasForeignKey<ExpenseReport>(x => x.RequestId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

      builder
      .HasOne(r => r.Requester)
      .WithMany(u => u.Requests)
      .HasForeignKey(u => u.RequesterId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);


      builder
      .HasOne(r => r.CurrentHandler)
      .WithMany(u => u.CurrentHandled)
      .HasForeignKey(u => u.CurrentHandlerId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

     builder
     .HasMany(r => r.Agents)
     .WithMany(a => a.Requests)
     .UsingEntity(j => j.ToTable("RequestAgents"));

    builder.Property(x => x.Activities)
    .HasColumnType("nvarchar(max)")
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<Activity>>(v, JsonSerializerOptions.Default)
    );


    builder.Property(x => x.Meetings)
    .HasColumnType("nvarchar(max)")
    .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<List<Meeting>>(v, JsonSerializerOptions.Default)
    );

    
     builder.Property(x => x.Budget)
     .HasColumnType("nvarchar(max)")
     .HasConversion(
      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
      v => JsonSerializer.Deserialize<RequestBudget>(v, JsonSerializerOptions.Default)
     );



    builder.Property(x => x.Objectives)
    .HasConversion(
     v => string.Join(',', v),
     v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());


    builder.Property(x => x.Items)
    .HasConversion(
    v => string.Join(',', v),
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());


    builder.Property(x => x.Personnel)
    .HasConversion(
    v => string.Join(',', v),
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());


    






    }
}