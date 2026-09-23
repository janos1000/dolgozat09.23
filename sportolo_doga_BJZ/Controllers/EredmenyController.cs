using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sportolo_doga_BJZ.Moduls.DTO;

namespace sportolo_doga_BJZ.Controllers
{
    [Route("/eredmeny")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=sportolo13b;uid=root;password=";

        [HttpGet]
        public List<Eredmeny> GetAllEredmeny()
        {
            List<Eredmeny> eredmenyek = new List<Eredmeny>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM eredmeny";

            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var eredmeny = new Eredmeny
                {
                    Id = dataReader.GetInt32(0),
                    Competition = dataReader.GetString(1),
                    Description = dataReader.GetString(2),
                    PostTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    SportoloId = dataReader.GetInt32(5)
                };
                eredmenyek.Add(eredmeny);
            }

            connector.Close();
            return eredmenyek;
        }

        [HttpPost]
        public object AddNewEredmeny(Eredmeny eredmeny)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var erd = new Eredmeny
            {
                Competition = eredmeny.Competition,
                Description = eredmeny.Description,
                PostTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            var sql = "INSERT INTO eredmeny (Competition, Description, PostTime, UpdateTime)" +
                "VALUES (@competition, @description, @postTime, @updateTime)";
            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@competition", erd.Competition);
            cmd.Parameters.AddWithValue("@description", erd.Description);
            cmd.Parameters.AddWithValue("@postTime", erd.PostTime);
            cmd.Parameters.AddWithValue("@updateTime", erd.UpdateTime);

            cmd.ExecuteNonQuery();
            connector.Close();
            return erd;
        }

        [HttpPut]
        public object UpdateEredmeny([FromQuery] int id, [FromBody] Eredmeny eredmeny)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"UPDATE `eredmeny` SET `competition` = @competition, `description` = @description WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@competition", eredmeny.Competition);
            cmd.Parameters.AddWithValue("@description", eredmeny.Description);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updateEredmeny = new Eredmeny
            {
                Competition = eredmeny.Competition,
                Description = eredmeny.Description
            };

            connector.Close();
            return new { message = "Sikeres frissítés.", result = updateEredmeny };
        }

        [HttpDelete]
        public object DeleteEredmeny(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "DELETE FROM eredmeny WHERE Id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            connector.Close();
            return new { message = "Sikeres törlés!" };
        }

        [HttpGet("byId")]
        public object GetEredmenyById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT `competition`, `description` FROM `eredmeny`
                        WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            datareader.Read();
            var eredmenybyid = new
            {
                Competitionme = datareader.GetString(0),
                Description = datareader.GetString(1)
            };

            connector.Close();
            return eredmenybyid;
        }
    }
}