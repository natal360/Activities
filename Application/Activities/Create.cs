using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Create
  {
    public class Command : IRequest
    {
      public Activity Activity { get; set; }
    }
    public class Hndler : IRequestHandler<Command>
    {
      private readonly DataContext _context;

      public Hndler(DataContext context)
      {
        _context = context;
      }
      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        _context.Activities.Add(request.Activity);

        await _context.SaveChangesAsync();

        return Unit.Value;
      }
    }
  }
}