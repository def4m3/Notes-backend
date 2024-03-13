using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler
        : IRequestHandler<DeleteNoteCommand>
    {
        private readonly INotesDbContext _context;

        public DeleteNoteCommandHandler(INotesDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Notes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId) { throw new NotFoundException(nameof(Note), request.Id); }

            _context.Notes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
