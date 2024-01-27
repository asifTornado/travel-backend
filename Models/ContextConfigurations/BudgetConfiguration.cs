using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

    }
    
    
}
