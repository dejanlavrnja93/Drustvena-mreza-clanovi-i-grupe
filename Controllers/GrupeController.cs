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
        private GroupDbRepository grupaDbRepository;
        private GrupaRepository grupaRepository;

        public GrupaController(GroupDbRepository grupaDbRepository, GrupaRepository grupaRepository)
        {
            this.grupaDbRepository = grupaDbRepository;
            this.grupaRepository = grupaRepository;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1|| pageSize < 1)
            {
                return BadRequest("Stranica i veličina stranice moraju biti veci od nule.");
            }
            try
            {
                List<Grupa> grupe = grupaDbRepository.GetPaged(page, pageSize);
                int totalCount = grupaDbRepository.CountAll();
                Object result = new
                {
                    Data = grupe,
                    TotalCount = totalCount
                };
                return Ok(result);
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
            try
            {
                if (string.IsNullOrEmpty(newGrupa.ImeGrupe))
                {
                    return BadRequest();
                }

                newGrupa.DatumOsnivanja = DateTime.Today;
                Grupa createdGrupa = grupaDbRepository.Create(newGrupa);

                return Ok(createdGrupa);
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


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {

                Grupa? grupa = grupaDbRepository.GetById(id);
                if (grupa == null)
                {
                    return NotFound();
                }
                grupaDbRepository.Delete(id);
                return NoContent();
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

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Grupa grupa)
        {
            try
            {


                if (string.IsNullOrEmpty(grupa.ImeGrupe))
                {
                    return BadRequest();
                }

                Grupa? postojecaGrupa = grupaDbRepository.GetById(id);

                if (postojecaGrupa == null)
                {
                    return NotFound();
                }

                grupa.Id = id;

                grupaDbRepository.Update(grupa);

                return NoContent();
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
    }
}