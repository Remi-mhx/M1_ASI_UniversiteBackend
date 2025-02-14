using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.Dtos;
using UniversiteDomain.UseCases.EtudiantUseCases.Get;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtudiantController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        // GET: api/<EtudiantController>
        [HttpGet]
        public async Task<ActionResult<List<EtudiantDto>>> GetAllEtudiants()
        {
            GetAllEtudiantsUseCase getAllEtudiantsUc = new GetAllEtudiantsUseCase(repositoryFactory);
            List<Etudiant> etudiants = null;
            
            try
            {
                etudiants = await getAllEtudiantsUc.ExecuteAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
                
            List<EtudiantDto> dtos = new List<EtudiantDto>();
            foreach (Etudiant etudiant in etudiants)
            {
                dtos.Add(new EtudiantDto().ToDto(etudiant));
            }
            
            return Ok(dtos);
        }

        // GET api/<EtudiantController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EtudiantDto>> GetEtudiant(long id)
        {
            GetEtudiantUseCase getEtudiantUc = new GetEtudiantUseCase(repositoryFactory);
            Etudiant etudiant = new Etudiant();
            try
            {
                etudiant = await getEtudiantUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            EtudiantDto dto = new EtudiantDto().ToDto(etudiant);
            return Ok(dto);
        }

        // Crée un nouvel étudiant sans parcours
        // POST api/<EtudiantController>
        [HttpPost]
        public async Task<ActionResult<EtudiantDto>> PostAsync([FromBody] EtudiantDto etudiantDto)
        {
            CreateEtudiantUseCase createEtudiantUc = new CreateEtudiantUseCase(repositoryFactory);           
            Etudiant etud = etudiantDto.ToEntity();
            try
            {
                etud = await createEtudiantUc.ExecuteAsync(etud);
            }
            catch (Exception e)
            {
                // On récupère ici les exceptions personnalisées définies dans la couche domain
                // Et on les envoie avec le code d'erreur 400 et l'intitulé "erreurs de validation"
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            EtudiantDto dto = new EtudiantDto().ToDto(etud);
            // On revoie la route vers le get qu'on n'a pas encore écrit!
            return CreatedAtAction(nameof(GetEtudiant), new { id = dto.Id }, dto);
        }
        
        // PUT api/<EtudiantController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EtudiantController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
