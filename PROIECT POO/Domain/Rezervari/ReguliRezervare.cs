namespace PROIECT_POO.Domain.Rezervari;

public class ReguliRezervare
{
    public TimeSpan DurataStandard { get; }
    public TimeSpan AnulareMinima { get; }

    public ReguliRezervare(TimeSpan durata, TimeSpan anulare)
    {
        DurataStandard = durata;
        AnulareMinima = anulare;
    }
}