using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        private readonly IMapper _mapper;

        public NoteController(IMapper mapper) => _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<NoteListVm>> GetAll()
        {
            var query = new GetNoteListQuery();
            query.UserId = this.UserId;

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteListVm>> Get(Guid id)
        {
            var query = new GetNoteDetailsQuery();
            query.UserId = this.UserId;
            query.Id = id;


            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
        {
            var cmd = _mapper.Map<CreateNoteCommand>(createNoteDto);
            cmd.UserId = this.UserId;
            var noteId = await Mediator.Send(cmd);

            return Ok(noteId);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var cmd = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            cmd.UserId = this.UserId;
            await Mediator.Send(cmd);

            return NoContent();
        }

        [HttpDelete("{id}")]
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
