using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Models.DataBaseEntities;
using MusicPlatform.Models.Implementations;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Services.Implementatiotion
{
    public class ChartBuilder : IChartBuilder
    {
        private readonly PlatformContext _context;

        public ChartBuilder(PlatformContext context)
            => _context = context;

        public async Task<Chart> GetChart()
        {
            var bestAlbums = await _context.Albums
                .Include(p => p.Artists)
                .Include(p => p.Tracks)
                .Include(p => p.UsersLikes)
                .ToListAsync();
            var artists = await _context.Artists
                .Include(p => p.FeaturingTracks)
                .ToListAsync();
            var tracks = await _context.Tracks
                .Include(p => p.Artists)
                .Include(p => p.Featuring)
                .Include(p => p.UsersLikes)
                .ToListAsync();

            return new()
            {
                Albums = bestAlbums
                    .OrderByDescending(album =>
                        album.Listnings)
                    .Take(10).ToList(),
                Tracks = tracks
                    .OrderByDescending(track =>
                        track.Listnings)
                    .Take(100).ToList(),
            };
        }

        private int ArtistsListnings(Artist artist)
            => artist.Releases.Sum(r => r.Listnings);
    }
}
