using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiTrainingProject.Data.Models;

namespace WebApiTrainingProject.Data.Models.Configurations
{
    public class NodeGraphConfiguration : IEntityTypeConfiguration<NodeGraph>
    {
        public void Configure(EntityTypeBuilder<NodeGraph> builder)
        {
            builder.ToTable("NodeGraphs");

            builder.HasKey(ng => ng.Id);

            builder.Property(ng => ng.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ng => ng.JsonData)
                .IsRequired();

            builder.HasOne(ng => ng.Project)
                .WithMany(p => p.NodeGraphs)
                .HasForeignKey(ng => ng.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}