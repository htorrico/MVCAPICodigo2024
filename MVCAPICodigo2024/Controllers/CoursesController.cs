using Microsoft.AspNetCore.Mvc;
using MVCAPICodigo2024.Models.Response;
using System.Text.Json;

namespace MVCAPICodigo2024.Controllers
{
    public class CoursesController : Controller
    {

        string url = "https://localhost:7238/api/Courses";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAsync(string filter)
        {
            HttpClient httpClient = new HttpClient();
            
            url = url + "?filter=" + filter;

            HttpResponseMessage response = await httpClient.GetAsync(url);

            List<CourseResponseV1> model = null;

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                model = JsonSerializer.Deserialize<List<CourseResponseV1>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                model = new List<CourseResponseV1>();
            }

            return Json(model);
        }
    }
}
