using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace sportolo_doga_BJZ.Controllers
{
    [Route("sportolo")]
    [ApiController]
    public class SportoloController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=sportolo13b;uid=root;password=";

        [HttpGet("info")]
        public IActionResult GetSportoloInfo([FromQuery] int id)
        {
            using (var connector = new MySqlConnection(ConnectionString))
            {
                connector.Open();
                string sql = "SELECT name, email FROM sportolo WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, connector))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Ok(new
                            {
                                Name = reader.GetString(0),
                                Email = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return NotFound(new { message = "A megadott ID-val nem található sportoló." });
        }

        [HttpGet("eredmeny-szama")]
        public IActionResult GetTotalEredmenyCount()
        {
            using (var connector = new MySqlConnection(ConnectionString))
            {
                connector.Open();
                string sql = "SELECT COUNT(*) FROM eredmeny";

                using (var cmd = new MySqlCommand(sql, connector))
                {
                    long count = Convert.ToInt64(cmd.ExecuteScalar());
                    return Ok(new { OsszesEredmeny = count });
                }
            }
        }
    }
}