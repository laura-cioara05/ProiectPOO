using PROIECT_POO.Domain.Common;

namespace PROIECT_POO.Domain.Rezervari;

public class Rezervare
{
    public Guid Id { get; }
    public Guid TerenId { get; }
    public Guid ClientId { get; }
    public IntervalOrar Interval { get; }   // adaugăm aici
    public RezervareStatus Status { get; }

    public Rezervare(Guid id, Guid terenId, Guid clientId,IntervalOrar interval, RezervareStatus status)
    {
        Id = id;
        TerenId = terenId;
        ClientId = clientId;
        Interval=interval;
        Status = status;
    }
}