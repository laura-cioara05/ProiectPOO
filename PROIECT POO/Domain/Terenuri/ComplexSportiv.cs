using PROIECT_POO.Domain.Exceptii;
using PROIECT_POO.Domain.Common;
using PROIECT_POO.Domain.Rezervari;
using PROIECT_POO.Domain.Terenuri;

namespace PROIECT_POO.Domain.Complex;

public class ComplexSportiv//coordonator/controller(Application Layer)
{
    private readonly List<TerenDeSport>_terenuri;
    private readonly List<Rezervare>_rezervari;
    private readonly ReguliRezervare _reguliRezervare;
    
    public IReadOnlyList<TerenDeSport> Terenuri => _terenuri.AsReadOnly();
    public IReadOnlyList<Rezervare>Rezervari => _rezervari.AsReadOnly();

    public ComplexSportiv(ReguliRezervare reguliRezervare)
    {
        _reguliRezervare = reguliRezervare;
        _terenuri=new List<TerenDeSport>();
        _rezervari = new List<Rezervare>();
    }

    
    //1.Administrarea terenurilor
    public void AdaugaTeren(TerenDeSport teren)
    {
        _terenuri.Add(teren);
    }

    public void StergeTeren(TerenDeSport teren)
    {
         var terenGasit = _terenuri.FirstOrDefault(t => t.Id == teren.Id);
         if (terenGasit == null)
             throw new Exception("Terenul nu exista!");
         
         //Se verifica daca exista rezervari active pentru terenul ales
         bool ExistaRezervariActive=_rezervari.Any(r=>r.Id==teren.Id && r.Status==RezervareStatus.Activa);

         if (ExistaRezervariActive)
             throw new Exception("Terenul nu poate fi sters daca are rezervari active!");

         _terenuri.Remove(teren);
    }

    public void StergeTerenuriDupaTip(TipTeren tip)
    {
        //Se verifica daca exista terenuri de acest tip 
        bool exista=_terenuri.Any(t => t.Tip == tip);

        if (!exista)
            throw new InvalidOperationException(
                $"Nu exista terenuri de tipul {tip}.");

        _terenuri.RemoveAll(t => t.Tip == tip);
    }
    
    public void ModificaProgramTeren(Guid terenId, TimeSpan oraDeschidereNoua, TimeSpan oraInchidereNoua)
    {
        var teren = _terenuri.FirstOrDefault(t => t.Id == terenId);
        if (teren == null) throw new Exception("Terenul nu exista.");

        teren.ModificaProgramFunctionare(oraDeschidereNoua, oraInchidereNoua);
    }

    public void AdaugaIntervalIndisponibilTeren(Guid terenId, IntervalOrar interval)
    {
        var teren = _terenuri.FirstOrDefault(t => t.Id == terenId);
        if (teren == null) throw new Exception("Terenul nu exista.");

        teren.AdaugaIntervalIndisponibil(interval);
    }

    public void StergeIntervalIndisponibilTeren(Guid terenId, IntervalOrar interval)
    {
        var teren = _terenuri.FirstOrDefault(t => t.Id == terenId);
        if (teren == null) throw new Exception("Terenul nu exista.");

        teren.StergeIntervalIndisponibil(interval);
    }


    
    public Rezervare CreeazaRezervare(Guid clientId, Guid terenId, IntervalOrar interval)
    {
        //Se gaseste terenul dupa terenId
        
        var teren = _terenuri.FirstOrDefault(t => t.Id == terenId);
        
        //Se verifica existenta terenului
        if (teren == null)
            throw new RezervareException("Terenul nu exista!");
        
        //Se verifica programul terenului
        if(!teren.Program.EsteDisponibil(interval))
        throw new RezervareException("Intervalul ales nu este disponibil in programul terenului!");
        
        //Se verifica suprapuneri cu alte rezervari
        bool suprapunere = _rezervari.Any(r => r.TerenId == terenId &&
                                               r.Status == RezervareStatus.Activa &&
                                               r.Interval.SeSuprapuneCu(interval));
        if (suprapunere)
            throw new RezervareException("Terenul este deja rezervat in intervalul ales!");
        
        //Se creeaza rezervarea dupa indeplinirea tuturor conditiilor de mai sus
        var rezervare = new Rezervare(Guid.NewGuid(), terenId, clientId, interval,RezervareStatus.Activa);
        _rezervari.Add(rezervare);
        return rezervare;
    }

    public void AnuleazaRezervare(Guid rezervareId, Guid clientId)
    {
        //Se cauta rezervarea
        var rezervare = _rezervari.FirstOrDefault(r=> r.Id == rezervareId);
        
        //Daca nu se gaseste in lista de rezervari, nu exista
        if(rezervare==null)
            throw new Exception("Rezervare nu exista!");
        
        //Persoana care doreste sa anuleze rezervarea nu este aceeasi care a si facut rezervarea
        if(rezervare.ClientId != clientId)
            throw new RezervareException("Clientul nu are dreptul de a anula rezervarea(Nu pe numele acesta este facuta rezervarea)!");

        //Timpul alocat anularii unei rezervari a expirat
        if ((rezervare.Interval.Start - DateTime.Now) < _reguliRezervare.AnulareMinima)
            throw new RezervareException("Nu se mai poate anula rezervarea,timpul acordat anularii a expirat!");

    }
}
