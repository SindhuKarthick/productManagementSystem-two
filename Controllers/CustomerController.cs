using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using productManagementSystem.Models;
using System.Web.Mvc;
using System.Data.Entity;

namespace productManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customer/SignIn
        public ActionResult SignIn()
        {
            // Check if the admin is already logged in
            if (Session["AdminUserId"] != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        // POST: Customer/SignIn
        [HttpPost]
        public ActionResult SignIn(string username, string password, String mail)
        {
            // Validate the sign-in credentials (admin credentials)
            var existingAdmin = db.Users.SingleOrDefault(u => u.Username == username);

            if (existingAdmin != null)
            {
                // If the admin exists, show an error message saying the admin already exists.
                ViewBag.Error = "Admin with this username already exists!";
                return View();
            }

            // Create a new admin user
            var newCustomer = new User
            {
                Username = username,
                PasswordHash = password,
                Role = "Customer",
                Email = mail

            };

            // Add new admin to the database
            db.Users.Add(newCustomer);
            db.SaveChanges();


            // Automatically log in the newly created admin and redirect to the dashboard
            Session["AdminUserId"] = newCustomer.UserId;
            Session["AdminUsername"] = newCustomer.Username;

            return RedirectToAction("Dashboard");
        }

        // GET: Customer/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == password && u.Role == "Customer");
            if (user != null)
            {
                Session["CustomerUserId"] = user.UserId;
                return RedirectToAction("Dashboard");
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // GET: Customer/Dashboard
        public ActionResult Dashboard()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        // POST: Customer/BuyProduct
        [HttpPost]
        public ActionResult BuyProduct(int productId)
        {
            var product = db.Products.Find(productId);
            if (product != null && product.StockCount > 0)
            {
                product.StockCount -= 1;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Product purchased successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Product is out of stock!";
            }
            return RedirectToAction("Dashboard");
        }

        // GET: Customer/Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}