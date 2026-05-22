namespace ChefBerlesAPI.Controllers
{
    using ChefBerlesAPI.Model.Dto;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ChefsController : ControllerBase
    {
        private static readonly HttpClient _http = new();

        private const string FirebaseUrl =
            "https://p161-7ddfd-default-rtdb.europe-west1.firebasedatabase.app/chefs.json";

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _http.GetAsync(FirebaseUrl);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Nem sikerult betolteni a sef adatokat.");

            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, ChefDto>>();
            var chefs = data?.Select(kv => new ChefDto
            {
                Id = int.Parse(kv.Key),
                Name = kv.Value.Name,
                Cuisine = kv.Value.Cuisine
            }).ToList() ?? new();

            return Ok(chefs);
        }
    }
}
