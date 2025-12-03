using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiTrainingProject.Data.Models;

namespace WebApiTrainingProject.Data.Models.Configurations
{
    public class CustomNodeTypeConfiguration : IEntityTypeConfiguration<CustomNodeType>
    {
        public void Configure(EntityTypeBuilder<CustomNodeType> builder)
        {
            builder.ToTable("CustomNodeTypes");

            builder.HasKey(nt => nt.Id);

            builder.Property(nt => nt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(nt => nt.Description)
                .HasMaxLength(500);

            builder.Property(nt => nt.InputDefinitions)
                .IsRequired();

            builder.Property(nt => nt.OutputDefinitions)
                .IsRequired();
        }
    }
}