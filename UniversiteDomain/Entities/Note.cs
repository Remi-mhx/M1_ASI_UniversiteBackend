namespace UniversiteDomain.Entities;

public class Note
{
    public long IdEtudiant { get; set; }
    public long IdUe { get; set; }
    public float Valeur { get; set; }
    public Etudiant? Etudiant { get; set; } = null;
    public Ue? Ue { get; set; } = null;

    public override string ToString()
    {
        return "Note de "+Valeur+" pour "+Etudiant+" en "+Ue;
    }
}