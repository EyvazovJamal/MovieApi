using Application.Booking;
using Domain.Common;
using Infrastructure.Repositories;
using Marten;

namespace Infrastructure.Booking;

public sealed class BookingRepository(IDocumentSession ctx, IDocumentStore documentStore)
    : AggregateRootRepository<Domain.Booking.Booking>(ctx, documentStore), IBookingRepository;
