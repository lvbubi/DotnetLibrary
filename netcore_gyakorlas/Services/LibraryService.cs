using System.Collections.Generic;
using System.Linq;
using EventApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using netcore_gyakorlas.Models;
using netcore_gyakorlas.UnitOfWork;

namespace netcore_gyakorlas.Services
{
    
    public interface ILibraryService
    {
    }
    
    public class LibraryService : AbstractService, ILibraryService
    {
        
        private readonly ILogger<LibraryService> _logger;
        
        public LibraryService(IUnitOfWork unitOfWork, ILogger<LibraryService> logger) : base(unitOfWork)
        {
            _logger = logger;
        }

        private void Log(string message)
        {
            _logger.LogInformation("PlaceService log: " + message);
        }
    }
}