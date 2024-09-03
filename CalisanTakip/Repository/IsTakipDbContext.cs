using System;
using System.Collections.Generic;
using CalisanTakip.Models;
using CalisanTakip.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CalisanTakip.Repository;

public partial class IsTakipDbContext : DbContext
{
    public IsTakipDbContext()
    {
    }

    public IsTakipDbContext(DbContextOptions<IsTakipDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Birimler> Birimlers { get; set; }

    public virtual DbSet<Durumlar> Durumlars { get; set; }

    public virtual DbSet<Isler> Islers { get; set; }

    public virtual DbSet<Personeller> Personellers { get; set; }

    public virtual DbSet<YetkiTurler> YetkiTurlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-D870JAD\\SQLEXPRESS;Initial Catalog=IsTakipDB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Isler>(entity =>
        {
            entity.Property(e => e.IsId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IsDurum).WithMany(p => p.Islers).HasConstraintName("FK_Isler_Durumlar");

            entity.HasOne(d => d.IsPersonel).WithMany(p => p.Islers).HasConstraintName("FK_Isler_Personeller");
        });

        modelBuilder.Entity<Personeller>(entity =>
        {
            entity.HasOne(d => d.PersonelYetkiTur).WithMany(p => p.Personellers).HasConstraintName("FK_Personeller_YetkiTurler");

            entity.HasOne(d => d.PersonlBirim).WithMany(p => p.Personellers).HasConstraintName("FK_Personeller_Birimler");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
