using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entities.Data;
using Entities.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace IdenticalNewsFeed2.Controllers
{
    public class NewsController : Controller
    {
        //The client uses the data from the api
        private readonly HttpClient _httpClient;
        //This is the base Uri for the api
        private Uri BaseEndPoint { get; set; }

        public NewsController(NewsDBContext context)
        {
            //This is the base path for the api.
            BaseEndPoint = new Uri("http://localhost:54095/api/News");
            //This sets up the new client.
            _httpClient = new HttpClient();
        }


        // GET: News
        public async Task<IActionResult> Index()
        {
            //Response uses the client to read data from the api. 
            var response = await _httpClient.GetAsync(BaseEndPoint, HttpCompletionOption.ResponseHeadersRead);
            //this throws a response if exception, this is especially important because of concurrency.
            response.EnsureSuccessStatusCode();
            //This turns the response body into a string.
            var data = await response.Content.ReadAsStringAsync();

            return View(JsonConvert.DeserializeObject<List<News>>(data));
        }

        // GET: News for viewers
        public async Task<IActionResult> ViewNews()
        {
            //Response uses the client to read data from the api. 
            var response = await _httpClient.GetAsync(BaseEndPoint, HttpCompletionOption.ResponseHeadersRead);
            //this throws a response if exception, this is especially important because of concurrency.
            response.EnsureSuccessStatusCode();
            //This turns the response body into a string.
            var data = await response.Content.ReadAsStringAsync();

            return View(JsonConvert.DeserializeObject<List<News>>(data));
        }

        //This does not work, but the api side works.
        // GET: News for viewers by Date
        //public async Task<IActionResult> 
        //    ViewNews([FromRoute] int startYear, [FromRoute] int startMonth, [FromRoute] int endYear, [FromRoute] int endMonth)
        //{
        //    //Response uses the client to read data from the api. 
        //    var response = await _httpClient.GetAsync(
        //        BaseEndPoint + $"/{startYear}/{startMonth}/{endYear}/{endMonth}", HttpCompletionOption.ResponseHeadersRead);
        //    //this throws a response if exception, this is especially important because of concurrency.
        //    response.EnsureSuccessStatusCode();
        //    //This turns the response body into a string.
        //    var data = await response.Content.ReadAsStringAsync();

        //    return View(JsonConvert.DeserializeObject<List<News>>(data));
        //}


        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await _httpClient.GetAsync(BaseEndPoint.ToString() + id, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            //this makes the Info from the clien readable. -- double check
            var news = JsonConvert.DeserializeObject<News>(data);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }


        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsId,Author,Title,Content,CreatedDate,UpdatedDate,HashTags")] News news)
        {
            if (ModelState.IsValid)
            {
                //This tells the api to post the data as Json.
                var response = await _httpClient.PostAsJsonAsync<News>(BaseEndPoint, news);

                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync(BaseEndPoint + $"/{id}", HttpCompletionOption.ResponseHeadersRead);
            //now it turns the response body into a string
            var data = await response.Content.ReadAsStringAsync();

            var news = JsonConvert.DeserializeObject<News>(data);

            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,Author,Title,Content,CreatedDate,UpdatedDate,HashTags")] News news)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync<News>(BaseEndPoint + $"/{id}", news);
                    response.EnsureSuccessStatusCode();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //needs to be !await as the method is async.
                    if (!await NewsExists(news.NewsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: News/Delete/5
        //You can't silence the truth!

        private async Task<bool> NewsExists(int id)
        {

            var response = await _httpClient.GetAsync(BaseEndPoint, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            //the context is being made into a list of news.
            var context = JsonConvert.DeserializeObject<List<News>>(data);

            return context.Any(e => e.NewsId == id);
        }
    }
}
