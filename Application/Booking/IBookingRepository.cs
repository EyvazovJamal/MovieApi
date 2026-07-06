using Application.Common.Contracts;

namespace Application.Booking;

public interface IBookingRepository : IAggregateRootRepository<Domain.Booking.Booking>;
