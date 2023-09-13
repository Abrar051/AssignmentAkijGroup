using AssignmentAkijGroup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text; // For StringBuilder
using System.ComponentModel.DataAnnotations;


namespace AssignmentAkijGroup.Controllers
{
    public class HomeController : Controller
    {


        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool UserExists(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE [User] = @User", connection))
                {
                    command.Parameters.AddWithValue("@User", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0; // Return true if the user already exists
                }
            }
        }




        [HttpPost]
        public string Register([FromBody] UserDetails userDetails)
        {
            // Perform server-side validation
            if (string.IsNullOrEmpty(userDetails.User) || userDetails.User.Length < 6 ||
                !System.Text.RegularExpressions.Regex.IsMatch(userDetails.User, "^[a-zA-Z0-9]*$") ||
                string.IsNullOrEmpty(userDetails.PassWord) || userDetails.PassWord.Length < 8)
            {
                
                ModelState.AddModelError("", "Please check your input.");
                if (userDetails.User.Length <= 6)
                {
                    return "User name should be 6 characters long";
                }
                else if (userDetails.PassWord.Length <= 8)
                {
                    return "Password should be 8 characters long";
                }
                else
                {
                    return "Please select user name of 6 characters and password of 8 characters";
                }

                //return View("Index", userDetails);  
                
            }

            // Check if the user already exists
            if (UserExists(userDetails.User))
            {
                // User already exists
                ModelState.AddModelError("User", "User already exists.");

                return "User already exists";
            }

            // Hash the password
            string hashedPassword = HashPassword(userDetails.PassWord);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("INSERT INTO UserDetails VALUES (@User, @PassWord, @Name, @Email, @Address)", connection))
                    {
                        command.Parameters.AddWithValue("@User", userDetails.User);
                        command.Parameters.AddWithValue("@PassWord", hashedPassword);
                        command.Parameters.AddWithValue("@Name", userDetails.Name);
                        command.Parameters.AddWithValue("@Email", userDetails.Email);
                        command.Parameters.AddWithValue("@Address", userDetails.Address);

                        int rowsAffected = command.ExecuteNonQuery();


                        return "Registration Successful";
                    }
                }
            }

            catch (Exception ex)
            {
                return ex.ToString(); // Exception view
            }

            
        }


        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }



    }
}
