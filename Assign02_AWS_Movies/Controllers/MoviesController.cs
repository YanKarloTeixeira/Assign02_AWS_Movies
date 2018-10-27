using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assign02_AWS_Movies.Data;
using Assign02_AWS_Movies.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Amazon.S3.Transfer;
using Assign02_AWS_Movies.Classes;
using Microsoft.AspNetCore.Http;
using Amazon.S3.Model;

namespace Assign02_AWS_Movies.Controllers
{
    public class MoviesController : Controller
    {
        private const string bucketName = "centennialbucket";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client = new AmazonS3Client(bucketRegion);
        private static IAmazonS3 Client = new AmazonS3Client(bucketRegion);
        private static string keyName = null;
        private static string filePath = null;

        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Movie.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> DownloadList()
        {
            var applicationDbContext = _context.Movie.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Index2(int? id)
        {
            var y = id;
            var applicationDbContext = _context.Movie.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

 
        [HttpPost("UploadFiles")]
        //public async Task PreCreate(String Title, string Description, int CategoryId, IFormFile FileName)
        public async Task<IActionResult> PreCreate(String Title, string Description, string CategoryName, int CategoryId, IFormFile FileName)
        {

            Movie movie = new Movie();
            movie.Title = Title;
            movie.description = Description;
            movie.CategoryId = CategoryId;
            movie.FileName = FileName.FileName.ToString();
            movie.Downloads = 0;
            movie.UploadDate = DateTime.Now;
            movie.User = this.User.Identity.Name;

            if (ModelState.IsValid)
            {

                _context.Add(movie);
                await _context.SaveChangesAsync();

                /////////////////////////////////////////////////
                //File transfering from user station to S3 Bucket
                /////////////////////////////////////////////////
                var temp = Path.GetTempFileName();
                using (var stream = new FileStream(temp, FileMode.Create))
                {
                    FileName.CopyTo(stream);
                }
                keyName = FileName.FileName;
                filePath = temp;
                UpLoadFileAsync().Wait();
                S3CannedACL acl = S3CannedACL.PublicRead;
                PutACLResponse response = await s3Client.PutACLAsync(new PutACLRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    CannedACL = acl
                });
                /* End of transfering *//////////////////////////

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName", movie.CategoryId);
            return View(movie);
        }

        private static async Task UpLoadFileAsync()
        {

            try
            {
                
                var fileTransferUtility = new TransferUtility(s3Client);
                
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine("Upload 2 concluded.");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encoutered on server. Message : '{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encoutered on server. Message : '{0}' when writing an object", e.Message);
            }
        }




        //// POST: Movies/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MovieId,Title,description,UploadDate,FileName,Downloads,User,CategoryId")] Movie movie)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(movie);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", movie.CategoryId);
        //    return View(movie);
        //}

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", movie.CategoryId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,description,UploadDate,FileName,Downloads,User,CategoryId")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", movie.CategoryId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            string  fileName = movie.FileName;
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            await MyBucket.FileDeleteAsync(fileName, bucketName, s3Client);
            return RedirectToAction(nameof(Index));

        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }
    }
}
