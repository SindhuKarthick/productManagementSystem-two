using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using productManagementSystem.Models;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Data.Entity.Validation;


namespace productManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/SignIn
        public ActionResult SignIn()
        {
            // Check if the admin is already logged in
            if (Session["AdminUserId"] != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        // POST: Admin/SignIn
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
            var newAdmin = new User
            {
                Username = username,
                PasswordHash = password,
                Role = "Admin",
                Email = mail

            };

            // Add new admin to the database
            db.Users.Add(newAdmin);
            db.SaveChanges();


            // Automatically log in the newly created admin and redirect to the dashboard
            Session["AdminUserId"] = newAdmin.UserId;
            Session["AdminUsername"] = newAdmin.Username;

            return RedirectToAction("Dashboard");
        }
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            ViewBag.Error = username + "-" + password;
            var user = db.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == password && u.Role == "Admin");
            if (user != null)
            {
                Session["AdminUserId"] = user.UserId;
                return RedirectToAction("Dashboard");
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            //  var products = db.Products.ToList();
            // return View(products);
            return View();
        }

        // GET: Admin/AddProduct
        public ActionResult AddProduct()
        {
            return View();
        }

        // POST: Admin/AddProduct
        [HttpPost]
        public ActionResult AddProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(model);
        }

        // GET: Admin/UpdateProduct/{id}
        public ActionResult UpdateProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        // POST: Admin/UpdateProduct/{id}
        [HttpPost]
        public ActionResult UpdateProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductDetails");
            }
            return View(model);
        }

        // GET: Admin/DeleteProduct/{id}
        public ActionResult DeleteProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        // POST: Admin/ConfirmDeleteProduct/{id}
        [HttpPost, ActionName("ConfirmDeleteProduct")]
        public ActionResult ConfirmDeleteProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("ProductDetails");
        }

        // GET: Admin/SearchProduct
        public ActionResult SearchProduct(string searchTerm)
        {
            var products = db.Products
                             .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                             .ToList();
            return View(products);
        }

        // GET: Admin/Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        // GET: Admin/ProductDetails
        public ActionResult ProductDetails()
        {
            // Get all products from the database
            var products = db.Products.ToList();

            // Pass the list of products to the view
            return View(products);  // This now uses the renamed "ProductDetails.cshtml"
        }
    }
}