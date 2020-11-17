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
        public Book GetByTitle(string title);
        IEnumerable<Book> GetByAuthor(int authorId);
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

        public Book GetByTitle(string title)
        {
            Log("GetByTitle(" + title + ")");
            return UnitOfWork.GetRepository<Book>().GetAsQueryable()
                .Where(book => book.Title.Equals(title))
                .Include(book => book.Author)
                .First();
        }

        public IEnumerable<Book> GetByAuthor(int authorId)
        {
            Log("GetByAuthor(" + authorId + ")");
            return UnitOfWork.GetRepository<Book>().GetAsQueryable()
                .Where(book => book.Author.Id.Equals(authorId))
                .Include(book => book.Author);
        }
    }
}