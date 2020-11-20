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
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(_bookService.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book newBook)
        {
            var book = _bookService.Create(newBook);
            return Created($"{book.Id}", book);
        }
        
        [HttpGet("{title}")]
        public ActionResult<Book> GetByTitle(string title)
        {
            return Ok(_bookService.GetByTitle(title));
        }
        
        [HttpGet("{authorId}")]
        public ActionResult<IEnumerable<Book>> GetByAuthor(int authorId)
        {
            return Ok(_bookService.GetByAuthor(authorId));
        }
        
        [HttpGet("{authorId}")]
        public ActionResult<IEnumerable<Book>> GetByYear(int year)
        {
            return Ok(_bookService.GetByYear(year));
        }
    }
}