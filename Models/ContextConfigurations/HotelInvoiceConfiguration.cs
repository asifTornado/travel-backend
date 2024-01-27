using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

namespace backEnd.Models.ContextConfigurations;

public class HotelInvoiceConfiguration : IEntityTypeConfiguration<HotelInvoice>
{
    public void Configure(EntityTypeBuilder<HotelInvoice> builder)
    {
        // Fluent API configuration for the "Product" entity
        // Add other configurations as needed
     
    }
}