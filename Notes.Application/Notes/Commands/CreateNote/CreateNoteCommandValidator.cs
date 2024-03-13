using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {

            RuleFor(cmd => cmd.Title).NotEmpty().MaximumLength(250);

            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);

        }
    }
}
