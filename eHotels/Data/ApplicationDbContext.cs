using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eHotels.Models;

namespace eHotels.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Amenity> Amenity { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Bookingarc> Bookingarc { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Damage> Damage { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Hotelchain> Hotelchain { get; set; }
        public virtual DbSet<Hotelchainemail> Hotelchainemail { get; set; }
        public virtual DbSet<Hotelchainphone> Hotelchainphone { get; set; }
        public virtual DbSet<Hotelphone> Hotelphone { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Renting> Renting { get; set; }
        public virtual DbSet<Rentingarc> Rentingarc { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Room> Room { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=DBProject;Username=postgres;Password=password1");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Amenity>(entity =>
            {
                entity.HasKey(e => e.Aid)
                    .HasName("amenity_pkey");

                entity.ToTable("amenity", "ehotel");

                entity.Property(e => e.Aid)
                    .HasColumnName("aid")
                    .HasDefaultValueSql("nextval('ehotel.amenity_aid_seq'::regclass)");

                entity.Property(e => e.Amenity1)
                    .IsRequired()
                    .HasColumnName("amenity");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.HasOne(d => d.R)
                    .WithMany(p => p.Amenity)
                    .HasForeignKey(d => d.Rid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("amenity_room_fkey");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Bid)
                    .HasName("booking_pkey");

                entity.ToTable("booking", "ehotel");

                entity.Property(e => e.Bid)
                    .HasColumnName("bid")
                    .HasDefaultValueSql("nextval('ehotel.booking_bid_seq'::regclass)");

                entity.Property(e => e.CustomerSsn).HasColumnName("customer_ssn");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.CustomerSsnNavigation)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.CustomerSsn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_customer_fkey");

                entity.HasOne(d => d.R)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.Rid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_room_fkey");
            });

            modelBuilder.Entity<Bookingarc>(entity =>
            {
                entity.HasKey(e => e.Baid)
                    .HasName("bookingarc_pkey");

                entity.ToTable("bookingarc", "ehotel");

                entity.Property(e => e.Baid)
                    .HasColumnName("baid")
                    .HasDefaultValueSql("nextval('ehotel.bookingarc_baid_seq'::regclass)");

                entity.Property(e => e.CustomerSsn).HasColumnName("customer_ssn");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.Property(e => e.StartDate).HasColumnName("start_date");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Ssn)
                    .HasName("customer_pkey");

                entity.ToTable("customer", "ehotel");

                entity.Property(e => e.Ssn)
                    .HasColumnName("ssn")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateOfEmployment)
                    .HasColumnName("date_of_employment")
                    .HasColumnType("date");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.Ssn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("customer_person_fkey");
            });

            modelBuilder.Entity<Damage>(entity =>
            {
                entity.HasKey(e => e.Did)
                    .HasName("damage_pkey");

                entity.ToTable("damage", "ehotel");

                entity.Property(e => e.Did)
                    .HasColumnName("did")
                    .HasDefaultValueSql("nextval('ehotel.damage_did_seq'::regclass)");

                entity.Property(e => e.Damage1)
                    .IsRequired()
                    .HasColumnName("damage");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.HasOne(d => d.R)
                    .WithMany(p => p.Damage)
                    .HasForeignKey(d => d.Rid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("damage_room_fkey");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Ssn)
                    .HasName("employee_pkey");

                entity.ToTable("employee", "ehotel");

                entity.Property(e => e.Ssn)
                    .HasColumnName("ssn")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateOfEmployment)
                    .HasColumnName("date_of_employment")
                    .HasColumnType("date");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);

                entity.HasOne(d => d.SsnNavigation)
                    .WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.Ssn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_person_fkey");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.Hid)
                    .HasName("hotel_pkey");

                entity.ToTable("hotel", "ehotel");

                entity.Property(e => e.Hid)
                    .HasColumnName("hid")
                    .HasDefaultValueSql("nextval('ehotel.hotel_hid_seq'::regclass)");

                entity.Property(e => e.AptNumber).HasColumnName("apt_number");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(355);

                entity.Property(e => e.HState)
                    .IsRequired()
                    .HasColumnName("h_state")
                    .HasMaxLength(50);

                entity.Property(e => e.Hcid).HasColumnName("hcid");

                entity.Property(e => e.HotelName)
                    .IsRequired()
                    .HasColumnName("hotel_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Manager).HasColumnName("manager");

                entity.Property(e => e.NumRooms).HasColumnName("num_rooms");

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("street_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StreetNumber).HasColumnName("street_number");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasMaxLength(6);

                entity.HasOne(d => d.Hc)
                    .WithMany(p => p.Hotel)
                    .HasForeignKey(d => d.Hcid)
                    .HasConstraintName("hotel_hotelchain_fkey");

                entity.HasOne(d => d.ManagerNavigation)
                    .WithMany(p => p.Hotel)
                    .HasForeignKey(d => d.Manager)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("hotel_employee_fkey");
            });

            modelBuilder.Entity<Hotelchain>(entity =>
            {
                entity.HasKey(e => e.Hcid)
                    .HasName("hotelchain_pkey");

                entity.ToTable("hotelchain", "ehotel");

                entity.Property(e => e.Hcid)
                    .HasColumnName("hcid")
                    .HasDefaultValueSql("nextval('ehotel.hotelchain_hcid_seq'::regclass)");

                entity.Property(e => e.AptNumber).HasColumnName("apt_number");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.HcState)
                    .IsRequired()
                    .HasColumnName("hc_state")
                    .HasMaxLength(50);

                entity.Property(e => e.HotelChainName)
                    .IsRequired()
                    .HasColumnName("hotel_chain_name")
                    .HasMaxLength(50);

                entity.Property(e => e.NumHotels).HasColumnName("num_hotels");

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("street_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StreetNumber).HasColumnName("street_number");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Hotelchainemail>(entity =>
            {
                entity.HasKey(e => new { e.Email, e.Hcid })
                    .HasName("hotelchainemail_pkey");

                entity.ToTable("hotelchainemail", "ehotel");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(355);

                entity.Property(e => e.Hcid).HasColumnName("hcid");

                entity.HasOne(d => d.Hc)
                    .WithMany(p => p.Hotelchainemail)
                    .HasForeignKey(d => d.Hcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("email_hotelchain_fkey");
            });

            modelBuilder.Entity<Hotelchainphone>(entity =>
            {
                entity.HasKey(e => new { e.PhoneNumber, e.Hcid })
                    .HasName("hotelchainphone_pkey");

                entity.ToTable("hotelchainphone", "ehotel");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(20);

                entity.Property(e => e.Hcid).HasColumnName("hcid");

                entity.HasOne(d => d.Hc)
                    .WithMany(p => p.Hotelchainphone)
                    .HasForeignKey(d => d.Hcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("phone_hotelchain_fkey");
            });

            modelBuilder.Entity<Hotelphone>(entity =>
            {
                entity.HasKey(e => new { e.PhoneNumber, e.Hid })
                    .HasName("hotelphone_pkey");

                entity.ToTable("hotelphone", "ehotel");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(20);

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.HasOne(d => d.H)
                    .WithMany(p => p.Hotelphone)
                    .HasForeignKey(d => d.Hid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("phone_hotel_fkey");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Ssn)
                    .HasName("person_pkey");

                entity.ToTable("person", "ehotel");

                entity.Property(e => e.Ssn)
                    .HasColumnName("ssn")
                    .ValueGeneratedNever();

                entity.Property(e => e.AptNumber).HasColumnName("apt_number");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(50);

                entity.Property(e => e.PState)
                    .IsRequired()
                    .HasColumnName("p_state")
                    .HasMaxLength(50);

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("street_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StreetNumber).HasColumnName("street_number");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("zip")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Renting>(entity =>
            {
                entity.HasKey(e => e.Rentid)
                    .HasName("renting_pkey");

                entity.ToTable("renting", "ehotel");

                entity.Property(e => e.Rentid)
                    .HasColumnName("rentid")
                    .HasDefaultValueSql("nextval('ehotel.renting_rentid_seq'::regclass)");

                entity.Property(e => e.CustomerSsn).HasColumnName("customer_ssn");

                entity.Property(e => e.EmployeeSsn).HasColumnName("employee_ssn");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.CustomerSsnNavigation)
                    .WithMany(p => p.Renting)
                    .HasForeignKey(d => d.CustomerSsn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_customer_fkey");

                entity.HasOne(d => d.EmployeeSsnNavigation)
                    .WithMany(p => p.Renting)
                    .HasForeignKey(d => d.EmployeeSsn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_employee_fkey");

                entity.HasOne(d => d.R)
                    .WithMany(p => p.Renting)
                    .HasForeignKey(d => d.Rid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_room_fkey");
            });

            modelBuilder.Entity<Rentingarc>(entity =>
            {
                entity.HasKey(e => e.Rentaid)
                    .HasName("rentingarc_pkey");

                entity.ToTable("rentingarc", "ehotel");

                entity.Property(e => e.Rentaid)
                    .HasColumnName("rentaid")
                    .HasDefaultValueSql("nextval('ehotel.rentingarc_rentaid_seq'::regclass)");

                entity.Property(e => e.CustomerSsn).HasColumnName("customer_ssn");

                entity.Property(e => e.EmployeeSsn).HasColumnName("employee_ssn");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.Property(e => e.StartDate).HasColumnName("start_date");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => new { e.Role1, e.EmployeeSsn })
                    .HasName("role_pkey");

                entity.ToTable("role", "ehotel");

                entity.Property(e => e.Role1)
                    .HasColumnName("role")
                    .HasMaxLength(100);

                entity.Property(e => e.EmployeeSsn).HasColumnName("employee_ssn");

                entity.HasOne(d => d.EmployeeSsnNavigation)
                    .WithMany(p => p.Role)
                    .HasForeignKey(d => d.EmployeeSsn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_employee_fkey");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Rid)
                    .HasName("room_pkey");

                entity.ToTable("room", "ehotel");

                entity.Property(e => e.Rid)
                    .HasColumnName("rid")
                    .HasDefaultValueSql("nextval('ehotel.room_rid_seq'::regclass)");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.Property(e => e.Isextandable).HasColumnName("isextandable");

                entity.Property(e => e.Landscape)
                    .IsRequired()
                    .HasColumnName("landscape")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("numeric(8,2)");

                entity.Property(e => e.RoomNum).HasColumnName("room_num");

                entity.HasOne(d => d.H)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.Hid)
                    .HasConstraintName("room_hotel_fkey");
            });

            modelBuilder.HasSequence<int>("amenity_aid_seq");

            modelBuilder.HasSequence<int>("booking_bid_seq");

            modelBuilder.HasSequence<int>("bookingarc_baid_seq");

            modelBuilder.HasSequence<int>("damage_did_seq");

            modelBuilder.HasSequence<int>("hotel_hid_seq");

            modelBuilder.HasSequence<int>("hotelchain_hcid_seq");

            modelBuilder.HasSequence<int>("renting_rentid_seq");

            modelBuilder.HasSequence<int>("rentingarc_rentaid_seq");

            modelBuilder.HasSequence<int>("room_rid_seq");
        }
    }
}
