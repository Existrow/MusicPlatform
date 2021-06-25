using System;
using Microsoft.EntityFrameworkCore;

namespace MusicPlatform.Models.DataBaseEntities
{
    public class PlatformContext : DbContext
    {
        public DbSet<Release> Releases { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<News> News { get; set; }

        public PlatformContext(DbContextOptions<PlatformContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => new { user.Login })
                .IsUnique(true);
            modelBuilder.Entity<Artist>()
                .HasIndex(artist => new { artist.Name })
                .IsUnique(true);

            modelBuilder.Entity<Track>()
                .HasMany(track => track.Albums)
                .WithMany(album => album.Tracks)
                .UsingEntity(j => j.ToTable("TracksAlbum"));
            modelBuilder.Entity<Track>()
                .HasMany(track => track.Featuring)
                .WithMany(artist => artist.FeaturingTracks)
                .UsingEntity(j => j.ToTable("TracksFeaturing"));
            modelBuilder.Entity<Release>()
                .HasMany(release => release.Artists)
                .WithMany(artist => artist.Releases)
                .UsingEntity(j => j.ToTable("ReleaseArtist"));
        }
    }
}
