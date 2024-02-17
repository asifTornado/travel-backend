using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {

       builder.HasMany(x => x.Users)
       .WithMany(x => x.Roles)
       .UsingEntity<UserRoles>(
         x => x.HasOne<User>().WithMany().HasForeignKey(e => e.UserId),
         y => y.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId)
       );



  


    }



}