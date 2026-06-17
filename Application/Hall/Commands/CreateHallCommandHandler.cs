using MediatR;

namespace Application.Hall.Commands;

public class CreateHallCommandHandler(IHallRepository hallRepository) :IRequestHandler<CreateHallCommand>
{
    public async Task Handle(CreateHallCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var hall = Domain.Hall.Hall.Create(id, request.Name, request.SeatCount);
        await hallRepository.StoreAsync(hall);
    }
}