namespace KalWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model11")
        {
        }

        public virtual DbSet<UploadData> UploadDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UploadData>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.Phone2)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.NewContact)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.GuarnatorMobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.BillingCycle)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.NPA_Stage)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.DPD)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.TL_Name)
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UploadData>()
                .Property(e => e.Pay_Freq)
                .IsUnicode(false);
        }
    }
}
