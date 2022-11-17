using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoplace.Common.Data.Services
{
    public interface IDataSeeder
    {
        Task SeedDataAsync();
    }
}
