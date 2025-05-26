using Microsoft.AspNetCore.Mvc;
using JokesApp.Data;
using JokesApp.Models;

namespace JokesApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm = "")
        {
            var jokes = _context.Jokes
                .Where(j => j.Question.ToLower().Contains(searchTerm.ToLower()))
                .ToList();

            ViewBag.SearchTerm = searchTerm;
            return View(jokes);
        }
    }
}