using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace backEnd.Models.ContextConfigurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotels>
{
    public void Configure(EntityTypeBuilder<Hotels> builder)
    {

        
      builder.Property(x => x.Rooms)
      .HasConversion(
        x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
        x => JsonSerializer.Deserialize<List<Room>>(x, JsonSerializerOptions.Default)
      );
    }



}