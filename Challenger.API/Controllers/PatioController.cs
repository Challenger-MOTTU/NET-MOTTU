using System.Net;
using Asp.Versioning;
using Challenger.Application.DTOs.Requests;
using Challenger.Application.DTOs.Responses;
using Challenger.Application.pagination;
using Challenger.Application.UseCase;
using Microsoft.AspNetCore.Mvc;
using Challenger.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly ICreatePatioUseCase _createPatioUseCase;
        private readonly IUpdatePatioUseCase _updatePatioUseCase;
        private readonly IPatioRepository _patioRepository;

        public PatioController(
            ICreatePatioUseCase createPatioUseCase,
            IUpdatePatioUseCase updatePatioUseCase,
            IPatioRepository patioRepository)
        {
            _createPatioUseCase = createPatioUseCase;
            _updatePatioUseCase = updatePatioUseCase;
            _patioRepository = patioRepository;
        }

        // GET: api/Patio
        /// <summary>
        /// Busca todas os patios cadastradas.
        /// </summary>
        /// <returns>
        /// Retorna 200 (OK) com a lista de patios.
        /// </returns>
        /// <response code="200">Retorna todos os patios.</response>

        [HttpGet]
        public async Task<IEnumerable<PatioResponse>> GetPatios()
        {
            var patios = await _patioRepository.GetAllAsync();
            return patios.Select(p => new PatioResponse(p.Id, p.Name, p.Cidade, p.Capacidade));
        }
        
        // GET: Pagination
        [HttpGet("paged")]
        public Task<PaginatedResult<PatioSummary>> GetPage([FromQuery] PageRequest pageRequest,
            [FromQuery] PatioQuery patioQuery)
        {
            return _createPatioUseCase.ExecuteAsync(pageRequest, patioQuery);
        }
        
        
        

        // GET: api/Patio/5
        /// <summary>
        /// Busca um pátio específico pelo ID.
        /// </summary>
        /// <param name="id">ID do pátio a ser buscado.</param>
        /// <returns>
        /// Retorna 200 (OK) com os dados do pátio, 
        /// ou 404 (Not Found) caso não exista.
        /// </returns>
        /// <response code="200">Retorna os dados do pátio.</response>
        /// <response code="404">Se o pátio não for encontrado.</response>
        /// <remarks>
        /// Exemplo de resposta (200 OK):
        /// 
        ///     {
        ///         "id": "2f7d05d6-4c93-4f0f-9e4b-7f41c34cb123",
        ///         "name": "Pátio Central",
        ///         "cidade": "São Paulo",
        ///         "capacidade": 150
        ///     }
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<PatioResponse>> GetPatio(Guid id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            if (patio == null) return NotFound();
            return new PatioResponse(patio.Id, patio.Name, patio.Cidade, patio.Capacidade);
        }

        // GET: api/Patios/cidade/{cidade}
        /// <summary>
        /// Busca todos os pátios cadastrados em uma cidade específica.
        /// </summary>
        /// <param name="cidade">Nome da cidade para filtrar os pátios.</param>
        /// <returns>
        /// Retorna 200 (OK) com a lista de pátios encontrados,
        /// ou 404 (Not Found) caso nenhum pátio esteja cadastrado na cidade informada.
        /// </returns>
        /// <response code="200">Retorna a lista de pátios da cidade.</response>
        /// <response code="404">Se nenhum pátio for encontrado na cidade informada.</response>
        /// <remarks>
        /// Exemplo de resposta (200 OK):
        /// 
        ///     [
        ///         {
        ///             "id": "a7f8c5d2-5c93-4f0f-9e4b-7f41c34cb123",
        ///             "name": "Pátio Zona Norte",
        ///             "cidade": "São Paulo",
        ///             "capacidade": 200
        ///         },
        ///         {
        ///             "id": "b9e5c179-3982-4c7c-bdcb-a3fda8998847",
        ///             "name": "Pátio Central",
        ///             "cidade": "São Paulo",
        ///             "capacidade": 150
        ///         }
        ///     ]
        /// </remarks>

        [HttpGet("cidade/{cidade}")]
        public async Task<ActionResult<IEnumerable<PatioResponse>>> GetPatiosPorCidade(string cidade)
        {
            var patios = await _patioRepository.GetByCidadeAsync(cidade);
            if (!patios.Any()) return NotFound($"Nenhum pátio encontrado para a cidade '{cidade}'.");
            return Ok(patios.Select(p => new PatioResponse(p.Id, p.Name, p.Cidade, p.Capacidade)));
        }

        // POST: api/Patios
        // POST: api/Patios
        /// <summary>
        /// Cria um novo pátio no sistema.
        /// </summary>
        /// <param name="request">Dados necessários para cadastrar o pátio.</param>
        /// <returns>
        /// Retorna 201 (Created) com os dados do pátio criado, 
        /// ou 400 (Bad Request) se a requisição for inválida.
        /// </returns>
        /// <response code="201">Retorna o pátio recém-criado.</response>
        /// <response code="400">Se os dados enviados forem inválidos.</response>
        /// <response code="500">Erro interno ao tentar criar o pátio.</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Patios
        ///     {
        ///         "name": "Pátio Leste",
        ///         "cidade": "São Paulo",
        ///         "capacidade": 120
        ///     }
        /// 
        /// Exemplo de resposta (201 Created):
        /// 
        ///     {
        ///         "id": "c2f7d05d6-4c93-4f0f-9e4b-7f41c34cb456",
        ///         "name": "Pátio Leste",
        ///         "cidade": "São Paulo",
        ///         "capacidade": 120
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(PatioResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PatioResponse>> PostPatio(PatioRequest request)
        {
            try
            {
                string createdBy = User?.Identity?.Name ?? "system";
                var patioResponse = await _createPatioUseCase.Execute(request, createdBy);
                return CreatedAtAction(nameof(GetPatio), new { id = patioResponse.Id }, patioResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao criar o pátio." });
            }
        }

        // PUT: api/Patios/{id}
        // PUT: api/Patios/{id}
        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        /// <param name="id">ID do pátio a ser atualizado.</param>
        /// <param name="request">Objeto contendo os novos dados do pátio.</param>
        /// <returns>
        /// Retorna 204 (NoContent) se a atualização for bem-sucedida,
        /// 404 (NotFound) se o pátio não for encontrado,
        /// 400 (BadRequest) se os dados enviados forem inválidos,
        /// ou 500 (Internal Server Error) em caso de falha inesperada.
        /// </returns>
        /// <response code="204">Pátio atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos enviados.</response>
        /// <response code="404">Pátio não encontrado.</response>
        /// <response code="500">Erro interno no servidor.</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     PUT /api/Patios/2f7d05d6-4c93-4f0f-9e4b-7f41c34cb123
        ///     {
        ///         "name": "Pátio Central",
        ///         "cidade": "Campinas",
        ///         "capacidade": 200
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PutPatio(Guid id, PatioRequest request)
        {
            try
            {
                // Pega o usuário autenticado, ou "system" se não houver
                string updatedBy = User?.Identity?.Name ?? "system";

                // Chama o UseCase com o novo parâmetro
                var updated = await _updatePatioUseCase.Execute(id, request, updatedBy);

                if (!updated)
                    return NotFound("Pátio não encontrado.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar o pátio." });
            }
        }
        // DELETE: api/Patios/5
        /// <summary>
        /// Remove um pátio existente do sistema.
        /// </summary>
        /// <param name="id">ID do pátio que será removido.</param>
        /// <returns>
        /// Retorna 204 (NoContent) se a exclusão for bem-sucedida,
        /// 404 (NotFound) se o pátio não for encontrado,
        /// ou 500 (Internal Server Error) em caso de falha inesperada.
        /// </returns>
        /// <response code="204">Pátio removido com sucesso.</response>
        /// <response code="404">Pátio não encontrado.</response>
        /// <response code="500">Erro interno no servidor.</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     DELETE /api/Patios/2f7d05d6-4c93-4f0f-9e4b-7f41c34cb123
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePatio(Guid id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            if (patio == null) return NotFound("Pátio não encontrado.");

            await _patioRepository.DeleteAsync(patio);
            return NoContent();
        }
        
        
    }
}
