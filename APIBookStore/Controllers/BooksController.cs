using APIBookStore.Models;
using APIBookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIBookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> GetById(string id)
        {
            var book = await _booksService.GetAsync(id);
            
            if(book == null) return NotFound();
        
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(GetById), new {id = newBook.Id}, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book bookUpdated)
        {
            var book = await _booksService.GetAsync(id);

            if(book == null) return NotFound();

            bookUpdated.Id = book.Id;   

            await _booksService.UpdateAsync(id, bookUpdated);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if(book == null) return NotFound();

            await _booksService.RemoveAsync(id);

            return NoContent();
        }
    }
}
