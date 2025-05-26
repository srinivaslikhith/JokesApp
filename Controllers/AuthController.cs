using Microsoft.AspNetCore.Mvc;
using JokesApp.Data;       // Access to the database context
using JokesApp.Models;     // Access to the User and Joke models

namespace JokesApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor: injects the database context so we can access the DB
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Register
        // This shows the registration form to the user
        [HttpGet]
        public IActionResult Register()
        {
            // Fetch all jokes from the DB to show as security questions
            ViewBag.Jokes = _context.Jokes.ToList();
            return View();
        }

        // POST: /Auth/Register
        // This handles form submission and saves the new user
        [HttpPost]
        public IActionResult Register(User user)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid().ToString(); // Assign a new ID
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            // Debug: show what failed
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }

            ViewBag.Jokes = _context.Jokes.ToList();
            return View(user);
        }
        // GET: /Auth/Login
        // Shows the login form
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        // This handles login form submission and validation
        [HttpPost]
        public IActionResult Login(string username, string password, string securityAnswer)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.UserName == username &&
                u.Password == password &&
                u.SecurityAnswer.ToLower() == securityAnswer.ToLower());

            if (user != null)
            {
                TempData["username"] = user.UserName;
                return RedirectToAction("Index", "Jokes");
            }

            ViewBag.Error = "Invalid credentials or security answer.";
            return View();
        }
    }
}