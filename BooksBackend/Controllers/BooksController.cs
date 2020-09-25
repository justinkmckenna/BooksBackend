using AutoMapper;
using AutoMapper.QueryableExtensions;
using BooksBackend.Domain;
using BooksBackend.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BooksBackend.Controllers
{
    public class BooksController : ControllerBase
    {
        BooksDataContext _context;
        IMapper _mapper;
        MapperConfiguration _mapperConfig;

        public BooksController(BooksDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpGet("books")]
        public async Task<ActionResult> GetAllBooks()
        {
            var response = new GetAllBooksResponse();
            var books = await _context.Books.Where(b => b.IsInInventory).
                ProjectTo<BooksResponseItem>(_mapperConfig).ToListAsync();
            response.Data = books;
            return Ok(response);
        }

        [HttpGet("books/{bookId:int}" , Name = "books#getbook")]
        public async Task<ActionResult> GetBook(int bookId)
        {
            var book = await _context.Books.Where(b => b.id == bookId && b.IsInInventory)
                .ProjectTo<GetBookDetailsResponse>(_mapperConfig).SingleOrDefaultAsync();
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost("books")]
        public async Task<ActionResult> CreateBook([FromBody] BookCreateRequest bookCreateRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var book = _mapper.Map<Book>(bookCreateRequest);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<GetBookDetailsResponse>(book);
            return CreatedAtRoute("books#getbook", new { bookId = response.Id }, response);
        }

        [HttpDelete("books/{bookId:int}")]
        public async Task<ActionResult> RemoveBook(int bookId)
        {
            var bookToRemove = await _context.Books.SingleOrDefaultAsync(b => b.id == bookId && b.IsInInventory);
            if (bookToRemove != null)
            {
                bookToRemove.IsInInventory = false;
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        [HttpPut("books/{bookId:int}/title")]
        public async Task<ActionResult> UpdateTitle(int bookId, [FromBody] string newTitle)
        {
            var book = await _context.Books.SingleOrDefaultAsync(x => x.id == bookId && x.IsInInventory);
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.Title = newTitle; // add validation to make sure it's not "it" title
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}