using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NoteController : BaseController
    {
        private readonly IMapper _mapper;

        public NoteController(IMapper mapper) => _mapper = mapper;
        /// <summary>
        /// Gets the list of notes
        /// </summary>
        /// <remarks>
        /// GET /note
        /// </remarks>
        /// <returns>Returs NoteListVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteListVm>> GetAll()
        {
            var query = new GetNoteListQuery();
            query.UserId = this.UserId;
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Gets the note by id
        /// </summary>
        /// <remarks>
        /// GET /note/DB1487CB-C37D-4934-A3D8-92FA925E2892
        /// </remarks>
        /// <param name="id">Note id (guid)</param>
        /// <returns>Returs NoteDetailsVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteListVm>> Get(Guid id)
        {
            var query = new GetNoteDetailsQuery();
            query.UserId = this.UserId;
            query.Id = id;


            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Creates the note
        /// </summary>
        /// <remarks>
        /// POST /note
        /// {
        ///     title: "note title",
        ///     details: "note details"
        /// }
        /// </remarks>
        /// <param name="createNoteDto">CreateNoteDto object</param>
        /// <returns>Returns id of created note (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
        {
            var cmd = _mapper.Map<CreateNoteCommand>(createNoteDto);
            cmd.UserId = this.UserId;
            var noteId = await Mediator.Send(cmd);

            return Ok(noteId);
        }

        /// <summary>
        /// Updates the note by id
        /// </summary>
        /// <remarks>
        /// PUT /note
        /// {
        ///     id: "DB1487CB-C37D-4934-A3D8-92FA925E2892",
        ///     title: "updated note title",
        /// }
        /// </remarks>
        /// <param name="updateNoteDto">UpdateNoteDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var cmd = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            cmd.UserId = this.UserId;
            await Mediator.Send(cmd);

            return NoContent();
        }

        /// <summary>
        /// Deletes the note by id
        /// </summary>
        /// <remarks>
        /// DELETE /note/3F2986B4-102E-4669-A0D0-F4F5D5135BE3
        /// </remarks>
        /// <param name="id">Id of the note (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var cmd = new DeleteNoteCommand();
            cmd.UserId = this.UserId;
            cmd.Id = Id;
            await Mediator.Send(cmd);

            return NoContent();
        }
    }
}
