using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netcore_gyakorlas.Models;
using netcore_gyakorlas.Services;

namespace netcore_gyakorlas.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public LibraryController(ILibraryService libraryService, IBookService bookService, IAuthorService authorService)
        {
            _libraryService = libraryService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(_bookService.GetAll());
        }
        
        [HttpGet("{title}")]
        public ActionResult<Book> GetByTitle(string title)
        {
            return Ok(_libraryService.GetByTitle(title));
        }
        
        [HttpGet("{authorId}")]
        public ActionResult<IEnumerable<Book>> GetByAuthor(int authorId)
        {
            return Ok(_libraryService.GetByAuthor(authorId));
        }
        
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author newAuthor)
        {
            var author = _authorService.Create(newAuthor);
            return Created($"{author.Id}", author);
        }
    }
}