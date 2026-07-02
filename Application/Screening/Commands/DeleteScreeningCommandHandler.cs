using MediatR;

namespace Application.Screening.Commands;

public class DeleteScreeningCommandHandler(IScreeningRepository screeningRepository) : IRequestHandler<DeleteScreeningCommand>
{
    public async Task Handle(DeleteScreeningCommand request, CancellationToken cancellationToken)
    {
        var screening=await screeningRepository.AggregateAsync(request.id);
        if (screening == null)
        {
            throw new Exception($"Screening with id {request.id} does not exist");
        }

        screening.Delete();
        screeningRepository.StoreAsync(screening);
    }
}