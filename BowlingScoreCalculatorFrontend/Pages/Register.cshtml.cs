using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BowlingScoreCalculatorFrontend.Pages
{
    public class RegisterModel(IHttpClientFactory clientFactory, ILogger<RegisterModel> logger) : PageModel
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly ILogger<RegisterModel> _logger = logger;

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var client = _clientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(Input), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7037/api/auth/register", content);  

                if (response.IsSuccessStatusCode)
                {
                    // Automatically log in the user after registration
                    var loginContent = new StringContent(JsonConvert.SerializeObject(new
                    {
                        Username = Input.Username,
                        Password = Input.Password
                    }), Encoding.UTF8, "application/json");

                    var loginResponse = await client.PostAsync("https://localhost:7037/api/auth/login", loginContent);

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        var token = await loginResponse.Content.ReadAsStringAsync();

                        Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.Now.AddMinutes(60)
                        });

                        return RedirectToPage("/BowlingForm");
                    }

                    ModelState.AddModelError(string.Empty, "Login failed after registration.");
                    return Page();
                }

                ModelState.AddModelError(string.Empty, "Registration failed.");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the registration.");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return Page();
            }
        }

        public class InputModel
        {
            [Required]
            public string Username { get; set; } = string.Empty; 

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty; 

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty; 
        }
    }
}
