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

        [HttpGet("{id}")]
        public ActionResult<GradeDto> Get(Guid id) {
            try {
                connect.connection.Open();

                string sql = "SELECT * FROM grades WHERE Id=@Id";

                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                cmd.Parameters.AddWithValue("Id", id);

                if (reader.Read()) {
                    var result = new GradeDto(
                        reader.GetGuid("Id"),
                        reader.GetInt32("Grade"),
                        reader.GetString("Description"),
                        reader.GetDateTime("Created")
                        );

                    connect.connection.Close();
                    return StatusCode(200, result);
                } else {
                    Exception e = new Exception();

                    connect.connection.Close();
                    return StatusCode(404, e.Message);
                }

            }
            catch (Exception) {

                return BadRequest(404);
            }
        }

        [HttpPost]
        public ActionResult<Jegyek> Post(CreateGradeDto createGrade) {
          
            var newGrade = new Jegyek {
                Id = Guid.NewGuid(),
                Grade = createGrade.Grade,
                Description = createGrade.Description,
                Created = DateTimeOffset.Now
            };

            try {

                connect.connection.Open();

                string sql = $"INSERT INTO `grades`(`Id`, ` Grade`, `Description`, `Created`) VALUES (@Id,@Grade,@Description,@Created)";
                
                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                cmd.Parameters.AddWithValue("Id", newGrade.Id);
                cmd.Parameters.AddWithValue("Grade", newGrade.Grade);
                cmd.Parameters.AddWithValue("Description", newGrade.Description);
                cmd.Parameters.AddWithValue("Created", newGrade.Created);
                cmd.ExecuteNonQuery();

                connect.connection.Close();
                return StatusCode(201, newGrade);

            }
            catch (Exception) {

                return BadRequest(404);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Jegyek> Put(UpdateGradeDto updateGrade, Guid id) {
            try {

                connect.connection.Open();

                string sql = $"UPDATE `grades` SET `Id`=@Id,`Grade`=@Grade,`Description`=@Description WHERE Id = @Id";

                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                cmd.Parameters.AddWithValue("Id", id);
                cmd.Parameters.AddWithValue("Grade", updateGrade.Grade);
                cmd.Parameters.AddWithValue("Description", updateGrade.Description);
                cmd.ExecuteNonQuery();

                connect.connection.Close();
                return StatusCode(204);
            }
            catch (Exception e) {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id) {
            try {
                connect.connection.Open();

                string sql = $"DELETE FROM grades WHERE Id = @Id";

                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                cmd.Parameters.AddWithValue("Id", id);
                cmd.ExecuteNonQuery();

                connect.connection.Close();
                return StatusCode(200);
            }
            catch (Exception e) {

                return BadRequest(e.Message);
            }


        }
    }
}
