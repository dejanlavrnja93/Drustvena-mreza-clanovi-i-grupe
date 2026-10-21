using Microsoft.AspNetCore.Mvc;
using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;

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
			List<Grupa> grupe =GrupaRepository.Data.Values.ToList();
            return Ok(grupe);
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