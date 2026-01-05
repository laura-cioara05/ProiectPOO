namespace PROIECT_POO.Domain.Common;

public sealed class IntervalOrar
{
    public DateTime Start { get; }
    public DateTime End { get; } 
    
    public IntervalOrar(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException("Sfârșitul trebuie să fie după început.");

        Start = start;
        End = end;
    }
    
    public bool SeSuprapuneCu(IntervalOrar alt)
    {
        return Start < alt.End && alt.Start < End;
    }
    public bool Contine(DateTime moment)
    {
        return Start <= moment && moment < End;
    }
    
}