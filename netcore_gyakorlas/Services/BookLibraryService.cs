using System.Collections.Generic;
using EventApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using netcore_gyakorlas.Models;
using netcore_gyakorlas.UnitOfWork;

namespace netcore_gyakorlas.Services
{
    
    public interface IBookLibraryService
    {
        IEnumerable<BookLibrary> GetAll();
        BookLibrary Create(BookLibrary newBookLibrary);
    }
    
    public class BookLibraryService : AbstractService, IBookLibraryService
    {
        
        private readonly ILogger<LibraryService> _logger;
        
        public BookLibraryService(IUnitOfWork unitOfWork, ILogger<LibraryService> logger) : base(unitOfWork)
        {
            _logger = logger;
        }

        private void Log(string message)
        {
            _logger.LogInformation("PlaceService log: " + message);
        }
        
        public IEnumerable<BookLibrary> GetAll()
        {
            Log("GetAll");
            return UnitOfWork.GetRepository<BookLibrary>().GetAll()
                .Include(bl => bl.Library)
                .Include(bl => bl.Book);

        }

        public BookLibrary Create(BookLibrary newBookLibrary)
        {
            Log("Create");
            newBookLibrary.Id = 0;
            UnitOfWork.GetRepository<BookLibrary>().Create(newBookLibrary);
            UnitOfWork.SaveChanges();
            return newBookLibrary;
        }
    }
}