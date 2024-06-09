using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BowlingScoreCalculatorFrontend.Pages
{
    public class LoginModel(IHttpClientFactory clientFactory) : PageModel
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(Input), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7037/api/auth/login", content);  

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(60)
                });

                return RedirectToPage("/BowlingForm");
            }

            ModelState.AddModelError(string.Empty, "Login failed.");
            return Page();
        }

        public class InputModel
        {
            [Required]
            public string Username { get; set; } = string.Empty;  

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;  
        }
    }
}
