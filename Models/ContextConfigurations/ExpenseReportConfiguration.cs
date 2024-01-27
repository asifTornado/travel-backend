using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class ExpenseReportConfiguration : IEntityTypeConfiguration<ExpenseReport>
{
    public void Configure(EntityTypeBuilder<ExpenseReport> builder)
    {

        
    builder.HasMany(x => x.Expenses)
    .WithOne(x => x.ExpenseReport)
    .HasForeignKey(x => x.ExpenseReportId)
    .OnDelete(DeleteBehavior.Cascade);
    
    }



}