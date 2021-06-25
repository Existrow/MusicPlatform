using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicPlatform.Models.Implementations;

namespace MusicPlatform.Services.Interfaces
{
    public interface IChartBuilder
    {
        public Task<Chart> GetChart();
    }
}
