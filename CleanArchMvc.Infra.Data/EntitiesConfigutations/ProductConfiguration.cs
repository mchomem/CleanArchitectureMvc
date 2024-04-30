namespace CleanArchMvc.Infra.Data.EntitiesConfigutations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnType("varchar")
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(200)
            .HasColumnType("varchar")
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasPrecision(10, 2);

        builder
            .Property(x => x.Stock)
            .IsRequired();

        builder
            .Property (x => x.Image)
            .HasMaxLength(250)
            .HasColumnType("varchar")
            .IsRequired(false);

        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }
}
