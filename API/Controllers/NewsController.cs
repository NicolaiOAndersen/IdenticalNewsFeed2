using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities.Data;
using Entities.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsDBContext _context;

        public NewsController(NewsDBContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public IEnumerable<News> Getnews()
        {
            return _context.news;
        }


        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Getnews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var news = await _context.news.FindAsync(id);

            if(news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        // GET: api/News/2019/1/2019/10
        [HttpGet("{startYear}/{startMonth}/{endYear}/{endMonth}")]
        public async Task<IActionResult> GetNewsByDate 
            ([FromRoute] int startYear, [FromRoute] int startMonth, [FromRoute] int endYear, [FromRoute] int endMonth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            DateTime startDate = new DateTime(startYear, startMonth, 1);
            DateTime endDate = new DateTime(endYear, endMonth, 28);
            //Jeg sætter endDate til den 28 da det er den kortest mulige måned, dog så kunne følgende måske virke:
            //new DateTime(endYear, endMonth, DateTime.MaxValue.Month)

            var news = _context.news.Where(item => item.UpdatedDate >= startDate && item.UpdatedDate <= endDate);

            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }


        // PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews([FromRoute] int id, [FromBody] News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != news.NewsId)
            {
                return BadRequest();
            }

            //Datetime needs to be fixed.
            DateTime today = new DateTime();
            today = DateTime.Now;

            news.UpdatedDate = today;

            _context.Entry(news).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/News
        [HttpPost]
        public async Task<IActionResult> PostNews([FromBody] News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //Datetime needs to be fixed.
            DateTime today = new DateTime();
            today = DateTime.Now;

            news.CreatedDate = today;
            news.UpdatedDate = today;

            _context.news.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.NewsId }, news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var news = await _context.news.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.news.Remove(news);
            await _context.SaveChangesAsync();

            return Ok(news);
        }

        private bool NewsExists(int id)
        {
            return _context.news.Any(e => e.NewsId == id);
        }
    }
}