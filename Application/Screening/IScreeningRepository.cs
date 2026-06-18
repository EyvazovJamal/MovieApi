using Application.Common.Contracts;

namespace Application.Screening;

public interface IScreeningRepository : IAggregateRootRepository<Domain.Screening.Screening>;
