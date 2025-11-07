using System.Net;
using Asp.Versioning;
using Challenger.Application.DTOs.Requests;
using Challenger.Application.DTOs.Responses;
using Challenger.Application.pagination;
using Challenger.Application.UseCase;
using Microsoft.AspNetCore.Mvc;
using Challenger.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication2.Controllers
{
    [Authorize]
    /// <inheritdoc />
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [SwaggerTag("Moto - CRUD operations")]
    [ApiController]
    public class MotoControllerV2(
        ICreateMotoUseCase createMotoUseCase,
        IUpdateMotoUseCase updateMotoUseCase,
        IMotoRepository motoRepository)
        : ControllerBase
    {
        // GET: api/
        /// <summary>
        /// Busca todas as motos cadastradas.
        /// </summary>
        /// <returns>
        /// Retorna 200 (OK) com a lista de motos.
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta (200 OK):
        /// 
        ///     [
        ///         {
        ///             "id": "2f7d05d6-4c93-4f0f-9e4b-7f41c34cb123",
        ///             "placa": "ABC1D23",
        ///             "modelo": 1,
        ///             "status": 1,
        ///             "patioId": "60e5c179-3982-4c7c-bdcb-a3fda8998847"
        ///         },
        ///         {
        ///             "id": "c6b2f64e-23a9-482d-b4f2-2fbc1e23b999",
        ///             "placa": "XYZ9E88",
        ///             "modelo": 3,
        ///             "status": 2,
        ///             "patioId": "70a5c179-3982-4c7c-bdcb-a3fda8991234"
        ///         }
        ///     ]
        /// </remarks>

        [HttpGet]
        [SwaggerOperation(Summary = "Get all yards", Description = "Returns a list of all yards")]
        [SwaggerResponse(StatusCodes.Status200OK, "Token returned")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error")]
        public async Task<IEnumerable<MotoResponse>> GetMotos()
        {
            var motos = await motoRepository.GetAllAsync();
            return motos.Select(m => new MotoResponse(
                m.Id, 
                m.Placa.Valor, 
                m.Modelo, 
                m.Status, 
                m.PatioId
            ));
            
        }

        [HttpGet("paged")]
        public Task<PaginatedResult<MotoSummary>> GetPage([FromQuery] PageRequest pageRequest,
            [FromQuery] MotoQuery motoQuery)
        {
            return createMotoUseCase.ExecuteAsync(pageRequest, motoQuery);
        }
            
            
        // GET: api/Moto/5
        /// <summary>
        /// busca as motos por id
        /// </summary>
        /// <param name="id">O ID da moto a ser consultada</param>
        /// <returns>
        /// Retorna 200 (OK) com os dados da moto, 
        /// ou 404 (NotFound) se não existir.
        /// </returns>
        /// <response code="200">Retorna os dados da moto.</response>
        /// <response code="404">Se a moto não for encontrada.</response>

        /// <remarks>
        /// Exemplo de resposta (200 OK):
        /// 
        ///     {
        ///         "id": "f0160bec-8658-463b-ac1b-238a60ffb8c7",
        ///         "placa": "rer-4546",
        ///         "modelo": 1,
        ///         "status": 2,
        ///         "patioId": "60e5c179-3982-4c7c-bdcb-a3fda8998847"
        ///     }
        /// 
        /// Exemplo de resposta (404 NotFound):
        /// 
        ///     {
        ///         "message": "Moto não encontrada"
        ///     }
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<MotoResponse>> GetMoto(Guid id)
        {
            var moto = await motoRepository.GetByIdAsync(id);
            if (moto == null) return NotFound();
            return new MotoResponse(
                moto.Id, 
                moto.Placa.Valor, 
                moto.Modelo, 
                moto.Status, 
                moto.PatioId
            );
        }

        // POST: api/Moto
        /// <summary>
        /// publica uma nova moto
        /// </summary>
        /// <param name="request">
        /// Objeto contendo os dados necessários para criar a moto
        /// </param>
        /// <returns>
        /// Retorna o objeto da moto criada com status 201 (created),
        /// 400 (BadRequest) se os dados forem inválios,
        /// ou 500 (Internal Server Error) em caso de erro inesperado
        /// </returns>
        /// <response code="201">Retorna a moto recém-criada.</response>
        /// <response code="400">Se os dados enviados forem inválidos.</response>
        /// <response code="500">Erro interno ao tentar criar o pátio.</response>        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     
        ///     {
        ///         "placa": "rer-4546",
        ///         "modelo": 1,
        ///         "status": 2,
        ///         "patioId": "60e5c179-3982-4c7c-bdcb-a3fda8998847"
        ///     }
        /// Exemplo de resposta (201 Created):
        /// 
        ///     {
        ///           "id": "f0160bec-8658-463b-ac1b-238a60ffb8c7",
        ///           "placa": "RER-4546",
        ///           "modelo": 1,
        ///           "status": 2,
        ///           "patioId": "60e5c179-3982-4c7c-bdcb-a3fda8998847"
        ///     }
        /// Exemplo de resposta (404 NotFound):
        /// 
        ///     {
        ///         "message": "Moto não encontrada"
        ///     }
        /// </remarks>
        [HttpPost]
        [SwaggerOperation(Summary = "Create new yard", Description = "Creates a new yard")]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto Created")]
        [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<MotoResponse>> PostMoto(MotoRequest request)
        {
            try
            {
                string createdBy = User?.Identity?.Name ?? "system";
                var motoResponse = await createMotoUseCase.Execute(request, createdBy);
                return CreatedAtAction(nameof(GetMoto), new { id = motoResponse.Id }, motoResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao criar a moto." });
            }
        }

        // PUT: api/Moto/{id}
        /// <summary>
        /// Atualiza a moto escolhida
        /// </summary>
        /// <param name="id">O ID da moto que será atualizada.</param>
        /// <param name="request">Objeto com os dados que serão atualizaos</param>
        /// <returns>
        /// Retorna NoContent (204) se a atualização for bem-sucedida,
        /// NotFound (404) se a moto não existir,
        /// BadRequest (400) se os dados fornecidos forem inválidos.
        /// ou 500 (Internal Server Error) em caso de falha inesperada.
        /// </returns>
        /// <response code="204">Moto atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos enviados.</response>
        /// <response code="404">Moto não encontrada.</response>
        /// <response code="500">Erro interno no servidor.</response>
        /// <remarks>
        ///  Exemplo de request:
        ///
        ///     PUT /api/moto
        ///     {
        ///         "placa": "rer-4546",
        ///         "modelo": 3,
        ///         "status": 1,
        ///         "patioId": "60e5c179-3982-4c7c-bdcb-a3fda8998847"
        ///     }
        /// Exemplo de resposta (404 NotFound):
        /// 
        ///     {
        ///         "message": "Moto não encontrada"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PutMoto(Guid id, MotoRequest request)
        {
            try
            {
                string updatedBy = User?.Identity?.Name ?? "system";
                var updated = await updateMotoUseCase.Execute(id, request, updatedBy);

                if (!updated)
                    return NotFound("Moto não encontrada.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar a moto." });
            }
        }

        // DELETE: api/Moto/5
        /// <summary>
        /// Deleta a moto escolhida
        /// </summary>
        /// <param name="id">O ID da moto que for escolhida</param>
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
        ///     DELETE /api/Motos/2f7d05d6-4c93-4f0f-9e4b-7f41c34cb123
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteMoto(Guid id)
        {
            var moto = await motoRepository.GetByIdAsync(id);
            if (moto == null) return NotFound("Moto não encontrada.");

            await motoRepository.DeleteAsync(moto);
            return NoContent();
        }

        
    }
}
