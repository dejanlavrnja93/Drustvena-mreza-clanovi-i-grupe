using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{

	[Route("api/grupe")]
	[ApiController]
	public class GrupaController : ControllerBase
	{
		private GrupaRepository grupaRepository = new GrupaRepository();

		[HttpGet]
		public ActionResult<List<Grupa>> GetAll()
		{
            List<Grupa> grupe = new List<Grupa>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=Data/mydatabase.db");
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string name = reader["Name"]?.ToString() ?? "";
                    DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);

                    Grupa grupa = new Grupa(id, name , creationDate);
                    grupe.Add(grupa);
                }

                connection.Close();
                return Ok(grupe);
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Greška pri konekciji ili izvršavanju neispravnih SQL upita.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Greška u konverziji podataka.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Konekcija nije otvorena ili je otvorena više puta:");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Neočekivana greška.");
            }

        }

		[HttpPost]
		public ActionResult<Grupa> Create([FromBody] Grupa newGrupa)
		{ 
			if(string.IsNullOrEmpty(newGrupa.ImeGrupe))
            {
                return BadRequest();
            }
            int newId = GenerateNewId(GrupaRepository.Data.Keys.ToList());
            newGrupa.Id = newId;
            newGrupa.DatumOsnivanja = DateTime.Today;
            GrupaRepository.Data[newId] = newGrupa;
			grupaRepository.Save();
            return NoContent();
        } 
		private int GenerateNewId(List<int> identifikatori)
        {
			int maxID = 0;
			foreach(int i in identifikatori)
            {
				if(i > maxID)
                {
					maxID = i;
                }
            }
			return maxID + 1;
        }

		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			if (!GrupaRepository.Data.ContainsKey(id))
			{
				return NotFound();
			}
			GrupaRepository.Data.Remove(id);
			grupaRepository.Save();
			return Ok();
		}
    }
}