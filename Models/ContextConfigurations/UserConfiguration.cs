using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backEnd.Models.ContextConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

         builder
         .Property( e => e.EmpId)
         .HasComputedColumnSql("REPLICATE('0', 7 - LEN(Id)) + CAST(Id AS VARCHAR(7))");



                        builder
        .HasOne(u => u.SuperVisor)
        .WithMany(u => u.SuperVised)
        .HasForeignKey(u => u.SuperVisorId)
           .OnDelete(DeleteBehavior.NoAction)
         .IsRequired(false);

        builder
        .HasOne(u => u.ZonalHead)
        .WithMany(u => u.Head)
        .HasForeignKey(u => u.ZonalHeadId)
           .OnDelete(DeleteBehavior.NoAction)
         .IsRequired(false);    


        builder
        .HasOne(u => u.TravelHandler)
        .WithMany(u => u.TravelHandled)
        .HasForeignKey(u => u.TravelHandlerId)
           .OnDelete(DeleteBehavior.NoAction)
         .IsRequired(false);


        builder
        .HasMany(u => u.FlyerNos)
        .WithOne(f => f.User)
        .HasForeignKey(f => f.UserId)
         .OnDelete(DeleteBehavior.Cascade)
         .IsRequired(false);



     builder.Property(x => x.Roles)
    .HasConversion(
     v => string.Join(',', v),
     v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());


     

  


    }



}