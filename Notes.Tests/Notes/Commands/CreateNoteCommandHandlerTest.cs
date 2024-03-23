using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Application.Notes.Commands.CreateNote;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Notes.Tests.Notes.Commands
{
    public class CreateNoteCommandHandlerTest : TestCommandBase
    {
        [Fact]
        public async Task CreateNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new CreateNoteCommandHandler(Context);
            var noteName = "note name";
            var noteDetails = "note details";

            //act
            var noteId = await handler.Handle(new CreateNoteCommand
            {
                Title = noteName,
                Details = noteDetails,
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

            //assert
            Assert.NotNull(await Context.Notes.SingleOrDefaultAsync(
                note => 
                note.Id == noteId
                && note.Title == noteName 
                && note.Details == noteDetails));

        }
    }
}
