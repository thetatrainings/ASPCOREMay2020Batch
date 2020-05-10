using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace L9.Models
{
    public partial class Ramadan2020Context : DbContext
    {
        public Ramadan2020Context()
        {
        }

        public Ramadan2020Context(DbContextOptions<Ramadan2020Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Product> Product { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=LENOVO\\SQLEXPRESS;Database=Ramadan2020;User ID=sa; Password=theta;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("text");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.LongDescription)
                    .HasColumnName("long_description")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("short_description")
                    .HasColumnType("text");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("text");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CurrentStock).HasColumnName("current_stock");

                entity.Property(e => e.Images)
                    .HasColumnName("images")
                    .HasColumnType("text");

                entity.Property(e => e.LongDescription)
                    .HasColumnName("long_description")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("short_description")
                    .HasColumnType("text");

                entity.Property(e => e.Sku)
                    .HasColumnName("sku")
                    .HasColumnType("text");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
