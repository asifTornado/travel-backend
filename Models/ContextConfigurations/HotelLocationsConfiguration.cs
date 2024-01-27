using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class HotelLocationsConfiguration : IEntityTypeConfiguration<HotelLocations>
{
    public void Configure(EntityTypeBuilder<HotelLocations> builder)
    {
        
         builder.HasMany(r => r.Hotels).WithOne(r => r.HotelLocation).HasForeignKey(h => h.HotelLocationsId)
         .OnDelete(DeleteBehavior.Cascade).IsRequired(false);


    }


}