using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expenses>
{
    public void Configure(EntityTypeBuilder<Expenses> builder)
    {
            
        
 
    }



}