using Microsoft.EntityFrameworkCore;

namespace Maikelvdb.Xprtz.Assessment.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Show>(builder => {
                builder.ToTable("Shows");

                builder.HasIndex(x => x.ExternalId).IsUnique();

                builder.HasQueryFilter(x => !x.IsArchived);
            });
        }

        #region Override SaveChanges

        public override int SaveChanges()
        {
            SetEntityDates();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetEntityDates();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetEntityDates();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetEntityDates();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetEntityDates()
        {
            var dbEntityType = typeof(IDbEntity);

            foreach (var entry in ChangeTracker.Entries())
            {
                var entryType = entry.Entity.GetType();
                if (entryType == dbEntityType)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            ((IDbEntity)entry.Entity).ModifiedDate = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }

        #endregion
    }
}