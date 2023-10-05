using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using JegyekAPI.Model;
using static JegyekAPI.DTOs;

namespace JegyekAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class JegyekController : ControllerBase {

        Connect connect = new Connect();
        private readonly List<GradeDto> grades = new();

        [HttpGet]
        public ActionResult<IEnumerable<GradeDto>> Get() {
            try {
                connect.connection.Open();

                string sql = "SELECT * FROM grades";

                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    var result = new GradeDto(
                        reader.GetGuid("Id"),
                        reader.GetInt32("Grade"),
                        reader.GetString("Description"),
                        reader.GetDateTime("Created")
                        );

                    grades.Add(result);
                }

                connect.connection.Close();

                return StatusCode(200, grades);
            }
            catch (Exception e) {

                return BadRequest(e.Message);
            }
        }
    }
}
