using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

namespace backEnd.Models.ContextConfigurations;

public class TicketInvoiceConfiguration : IEntityTypeConfiguration<TicketInvoice>
{
    public void Configure(EntityTypeBuilder<TicketInvoice> builder)
    {
        // Fluent API configuration for the "Product" entity
        // Add other configurations as needed
     
    }
}