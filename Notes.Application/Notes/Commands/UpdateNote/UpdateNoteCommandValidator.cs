using FluentValidation;
using Notes.Application.Notes.Commands.CreateNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {

            RuleFor(cmd => cmd.Title).NotEmpty().MaximumLength(250);

            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);

            RuleFor(cmd => cmd.Id).NotEqual(Guid.Empty);

        }
    }
}
