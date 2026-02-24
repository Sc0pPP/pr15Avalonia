using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace pr15Avalonia.Models;

public partial class PcDbContext : DbContext
{
    public PcDbContext()
    {
    }

    public PcDbContext(DbContextOptions<PcDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assembly> Assemblies { get; set; }

    public virtual DbSet<Basepart> Baseparts { get; set; }

    public virtual DbSet<Boardformfactorcase> Boardformfactorcases { get; set; }

    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<Casesize> Casesizes { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Cpu> Cpus { get; set; }

    public virtual DbSet<Fandimension> Fandimensions { get; set; }

    public virtual DbSet<Formfactor> Formfactors { get; set; }

    public virtual DbSet<Gpu> Gpus { get; set; }

    public virtual DbSet<Gpuinterface> Gpuinterfaces { get; set; }

    public virtual DbSet<Hdd> Hdds { get; set; }

    public virtual DbSet<Igpu> Igpus { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Memorytype> Memorytypes { get; set; }

    public virtual DbSet<Motherboard> Motherboards { get; set; }

    public virtual DbSet<Partassembly> Partassemblies { get; set; }

    public virtual DbSet<Parttype> Parttypes { get; set; }

    public virtual DbSet<Powersupply> Powersupplies { get; set; }

    public virtual DbSet<Processorcooler> Processorcoolers { get; set; }

    public virtual DbSet<Ram> Rams { get; set; }

    public virtual DbSet<Socket> Sockets { get; set; }

    public virtual DbSet<Socketprocessorcooler> Socketprocessorcoolers { get; set; }

    public virtual DbSet<Ssd> Ssds { get; set; }

    public virtual DbSet<Storagedevice> Storagedevices { get; set; }

    public virtual DbSet<Storagedeviceinterface> Storagedeviceinterfaces { get; set; }

    public virtual DbSet<Storagedevicetype> Storagedevicetypes { get; set; }

    public virtual DbSet<Videoconnector> Videoconnectors { get; set; }

    public virtual DbSet<Videoconnectorgpu> Videoconnectorgpus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-ABCR8CG;Database=15prPC;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assembly>(entity =>
        {
            entity.ToTable("assembly$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Basepart>(entity =>
        {
            entity.ToTable("basepart$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Parttypeid).HasColumnName("parttypeid");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Baseparts)
                .HasForeignKey(d => d.Manufacturerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_basepart$_manufacturer$");

            entity.HasOne(d => d.Parttype).WithMany(p => p.Baseparts)
                .HasForeignKey(d => d.Parttypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_basepart$_parttype$");
        });

        modelBuilder.Entity<Boardformfactorcase>(entity =>
        {
            entity.ToTable("boardformfactorcase$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Caseid).HasColumnName("caseid");
            entity.Property(e => e.Formfactorid).HasColumnName("formfactorid");

            entity.HasOne(d => d.Case).WithMany(p => p.Boardformfactorcases)
                .HasForeignKey(d => d.Caseid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_boardformfactorcase$_case$");

            entity.HasOne(d => d.Formfactor).WithMany(p => p.Boardformfactorcases)
                .HasForeignKey(d => d.Formfactorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_boardformfactorcase$_formfactor$");
        });

        modelBuilder.Entity<Case>(entity =>
        {
            entity.ToTable("case$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Expansionslots).HasColumnName("expansionslots");
            entity.Property(e => e.Fans).HasColumnName("fans");
            entity.Property(e => e.Sizeid).HasColumnName("sizeid");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Case)
                .HasForeignKey<Case>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_case$_basepart$");

            entity.HasOne(d => d.Size).WithMany(p => p.Cases)
                .HasForeignKey(d => d.Sizeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_case$_casesize$");
        });

        modelBuilder.Entity<Casesize>(entity =>
        {
            entity.ToTable("casesize$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.ToTable("certificate$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Cpu>(entity =>
        {
            entity.ToTable("cpu$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Basecorefrequency).HasColumnName("basecorefrequency");
            entity.Property(e => e.Cachel3).HasColumnName("cachel3");
            entity.Property(e => e.Hasigpu).HasColumnName("hasigpu");
            entity.Property(e => e.Igpuid).HasColumnName("igpuid");
            entity.Property(e => e.Maxcorefrequency).HasColumnName("maxcorefrequency");
            entity.Property(e => e.Numberofcores).HasColumnName("numberofcores");
            entity.Property(e => e.Socketid).HasColumnName("socketid");
            entity.Property(e => e.Thermalpower).HasColumnName("thermalpower");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Cpu)
                .HasForeignKey<Cpu>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cpu$_basepart$");

            entity.HasOne(d => d.Igpu).WithMany(p => p.Cpus)
                .HasForeignKey(d => d.Igpuid)
                .HasConstraintName("FK_cpu$_igpu$");

            entity.HasOne(d => d.Socket).WithMany(p => p.Cpus)
                .HasForeignKey(d => d.Socketid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cpu$_socket$1");
        });

        modelBuilder.Entity<Fandimension>(entity =>
        {
            entity.ToTable("fandimension$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Formfactor>(entity =>
        {
            entity.ToTable("formfactor$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gpu>(entity =>
        {
            entity.ToTable("gpu$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Chipfrequency).HasColumnName("chipfrequency");
            entity.Property(e => e.Gpuinterfaceid).HasColumnName("gpuinterfaceid");
            entity.Property(e => e.Memorybus).HasColumnName("memorybus");
            entity.Property(e => e.Recommendpower).HasColumnName("recommendpower");
            entity.Property(e => e.Videomemory).HasColumnName("videomemory");

            entity.HasOne(d => d.Gpuinterface).WithMany(p => p.Gpus)
                .HasForeignKey(d => d.Gpuinterfaceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_gpu$_gpuinterface$");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Gpu)
                .HasForeignKey<Gpu>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_gpu$_basepart$");
        });

        modelBuilder.Entity<Gpuinterface>(entity =>
        {
            entity.ToTable("gpuinterface$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Hdd>(entity =>
        {
            entity.ToTable("hdd$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Rotationspeed).HasColumnName("rotationspeed");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Hdd)
                .HasForeignKey<Hdd>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hdd$_storagedevice$");
        });

        modelBuilder.Entity<Igpu>(entity =>
        {
            entity.ToTable("igpu$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("manufacturer$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Memorytype>(entity =>
        {
            entity.ToTable("memorytype$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Motherboard>(entity =>
        {
            entity.ToTable("motherboard$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Formfactorid).HasColumnName("formfactorid");
            entity.Property(e => e.Memoryslots).HasColumnName("memoryslots");
            entity.Property(e => e.Memorytypeid).HasColumnName("memorytypeid");
            entity.Property(e => e.Pcislots).HasColumnName("pcislots");
            entity.Property(e => e.Sataports).HasColumnName("sataports");
            entity.Property(e => e.Socketid).HasColumnName("socketid");
            entity.Property(e => e.Usbports).HasColumnName("usbports");

            entity.HasOne(d => d.Formfactor).WithMany(p => p.Motherboards)
                .HasForeignKey(d => d.Formfactorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_motherboard$_formfactor$");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Motherboard)
                .HasForeignKey<Motherboard>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_motherboard$_basepart$");

            entity.HasOne(d => d.Memorytype).WithMany(p => p.Motherboards)
                .HasForeignKey(d => d.Memorytypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_motherboard$_memorytype$");

            entity.HasOne(d => d.Socket).WithMany(p => p.Motherboards)
                .HasForeignKey(d => d.Socketid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_motherboard$_socket$");
        });

        modelBuilder.Entity<Partassembly>(entity =>
        {
            entity.ToTable("partassembly$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Assemblyid).HasColumnName("assemblyid");
            entity.Property(e => e.Partid).HasColumnName("partid");

            entity.HasOne(d => d.Assembly).WithMany(p => p.Partassemblies)
                .HasForeignKey(d => d.Assemblyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_partassembly$_assembly$");

            entity.HasOne(d => d.Part).WithMany(p => p.Partassemblies)
                .HasForeignKey(d => d.Partid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_partassembly$_basepart$");
        });

        modelBuilder.Entity<Parttype>(entity =>
        {
            entity.ToTable("parttype$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Powersupply>(entity =>
        {
            entity.ToTable("powersupply$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Certificationid).HasColumnName("certificationid");
            entity.Property(e => e.Fandimensionid).HasColumnName("fandimensionid");
            entity.Property(e => e.Power).HasColumnName("power");

            entity.HasOne(d => d.Certification).WithMany(p => p.Powersupplies)
                .HasForeignKey(d => d.Certificationid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_powersupply$_certificate$");

            entity.HasOne(d => d.Fandimension).WithMany(p => p.Powersupplies)
                .HasForeignKey(d => d.Fandimensionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_powersupply$_fandimension$");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Powersupply)
                .HasForeignKey<Powersupply>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_powersupply$_basepart$");
        });

        modelBuilder.Entity<Processorcooler>(entity =>
        {
            entity.ToTable("processorcooler$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Fandimensionid).HasColumnName("fandimensionid");
            entity.Property(e => e.Heatpipes).HasColumnName("heatpipes");
            entity.Property(e => e.Maxspeed).HasColumnName("maxspeed");
            entity.Property(e => e.Minspeed).HasColumnName("minspeed");
            entity.Property(e => e.Noiselevel).HasColumnName("noiselevel");

            entity.HasOne(d => d.Fandimension).WithMany(p => p.Processorcoolers)
                .HasForeignKey(d => d.Fandimensionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_processorcooler$_fandimension$");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Processorcooler)
                .HasForeignKey<Processorcooler>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_processorcooler$_basepart$");
        });

        modelBuilder.Entity<Ram>(entity =>
        {
            entity.ToTable("ram$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Ghz).HasColumnName("ghz");
            entity.Property(e => e.Memorytypeid).HasColumnName("memorytypeid");
            entity.Property(e => e.Timings)
                .HasMaxLength(255)
                .HasColumnName("timings");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Ram)
                .HasForeignKey<Ram>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ram$_basepart$");

            entity.HasOne(d => d.Memorytype).WithMany(p => p.Rams)
                .HasForeignKey(d => d.Memorytypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ram$_memorytype$");
        });

        modelBuilder.Entity<Socket>(entity =>
        {
            entity.ToTable("socket$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Socketprocessorcooler>(entity =>
        {
            entity.ToTable("socketprocessorcooler$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Processorcoolerid).HasColumnName("processorcoolerid");
            entity.Property(e => e.Socketid).HasColumnName("socketid");

            entity.HasOne(d => d.Processorcooler).WithMany(p => p.Socketprocessorcoolers)
                .HasForeignKey(d => d.Processorcoolerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_socketprocessorcooler$_processorcooler$");

            entity.HasOne(d => d.Socket).WithMany(p => p.Socketprocessorcoolers)
                .HasForeignKey(d => d.Socketid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_socketprocessorcooler$_socket$");
        });

        modelBuilder.Entity<Ssd>(entity =>
        {
            entity.ToTable("ssd$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Tbw).HasColumnName("tbw");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Ssd)
                .HasForeignKey<Ssd>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ssd$_storagedevice$");
        });

        modelBuilder.Entity<Storagedevice>(entity =>
        {
            entity.ToTable("storagedevice$");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Storagedeviceinterfaceid).HasColumnName("storagedeviceinterfaceid");
            entity.Property(e => e.Storagedevicetypeid).HasColumnName("storagedevicetypeid");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Storagedevice)
                .HasForeignKey<Storagedevice>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_storagedevice$_basepart$");

            entity.HasOne(d => d.Storagedeviceinterface).WithMany(p => p.Storagedevices)
                .HasForeignKey(d => d.Storagedeviceinterfaceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_storagedevice$_storagedeviceinterface$");

            entity.HasOne(d => d.Storagedevicetype).WithMany(p => p.Storagedevices)
                .HasForeignKey(d => d.Storagedevicetypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_storagedevice$_storagedevicetype$");
        });

        modelBuilder.Entity<Storagedeviceinterface>(entity =>
        {
            entity.ToTable("storagedeviceinterface$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Storagedevicetype>(entity =>
        {
            entity.ToTable("storagedevicetype$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Videoconnector>(entity =>
        {
            entity.ToTable("videoconnector$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Videoconnectorgpu>(entity =>
        {
            entity.ToTable("videoconnectorgpu$");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Gpuid).HasColumnName("gpuid");
            entity.Property(e => e.Videoconnectorid).HasColumnName("videoconnectorid");

            entity.HasOne(d => d.Gpu).WithMany(p => p.Videoconnectorgpus)
                .HasForeignKey(d => d.Gpuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_videoconnectorgpu$_gpu$");

            entity.HasOne(d => d.Videoconnector).WithMany(p => p.Videoconnectorgpus)
                .HasForeignKey(d => d.Videoconnectorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_videoconnectorgpu$_videoconnector$");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
