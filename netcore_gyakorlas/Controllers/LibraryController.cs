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
    }
}