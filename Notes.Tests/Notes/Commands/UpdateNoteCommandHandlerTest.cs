using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTest : TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var updatedTitle = "new title";

            //Act
            await handler.Handle(new UpdateNoteCommand()
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserBId,
                Title = updatedTitle,
            }, CancellationToken.None);

            //Assert
            Assert.NotNull(await Context.Notes.SingleOrDefaultAsync(
                note => 
                note.Title == updatedTitle
                && note.Id == NotesContextFactory.NoteIdForUpdate));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);

            //act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(new UpdateNoteCommand()
            {
                Id = Guid.NewGuid(),
                UserId = NotesContextFactory.UserAId,
            }, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var updateHandler = new UpdateNoteCommandHandler(Context);

            //act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(new UpdateNoteCommand()
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserAId,
            }, CancellationToken.None));
        }   
    }
}
