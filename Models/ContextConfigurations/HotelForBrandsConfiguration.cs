using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class HotelForBrandsConfiguration : IEntityTypeConfiguration<HotelForBrands>
{
    public void Configure(EntityTypeBuilder<HotelForBrands> builder)
    {

        
    builder.HasMany(r => r.Locations).WithOne(r => r.HotelForBrands).HasForeignKey(h => h.HotelForBrandsId)
         .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
    }



}