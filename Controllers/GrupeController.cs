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
		private GroupDbRepository grupaDbRepository = new GroupDbRepository();
        private GrupaRepository grupaRepository = new GrupaRepository();


        [HttpGet]
		public ActionResult<List<Grupa>> GetAll()
		{
            
            try
            {
                List<Grupa> grupe = grupaDbRepository.GetAll();
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
        [HttpGet("{id}")]
        public ActionResult<Grupa> GetById(int id)
        {
            try     
            {
                Grupa? grupa = grupaDbRepository.GetById(id);
                if (grupa == null)
                {
                    return NotFound();
                }
                return Ok(grupa);
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