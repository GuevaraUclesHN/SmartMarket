using SmartMarket.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Interfaces 
{
    public interface IProviderManagementService : IDisposable
    {
        Task<Provider> GetFromApiByIdAsync(Guid providerId);
    }
}
