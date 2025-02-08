using Microsoft.AspNetCore.Mvc;
using MVCAPICodigo2024.Models.Request;
using MVCAPICodigo2024.Models.Response;
using System.Text;
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

        [HttpPost]
        public async Task<JsonResult> CreateAsync(string name)
        {

            try
            {
                CourseRequestV1 request = new CourseRequestV1 { Name = name };
                using HttpClient client = new HttpClient();
                
                string jsonRequest = JsonSerializer.Serialize(request);

                HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + jsonResponse);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }


                return Json(new { message = "Escuela registrada Correctamente" });
            }
            catch (Exception)
            {

                return Json(new { message = "Error, comunicarse con el adminstrador" });
            }

        }
    }
}
