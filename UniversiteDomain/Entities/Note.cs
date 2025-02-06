namespace UniversiteDomain.Entities;

public class Note
{
    public float Valeur { get; set; }
    public long IdEtudiant { get; set; }
    public long IdUe { get; set; }
    public Etudiant Etudiant { get; set; }
    public Ue Ue { get; set; }
        
    public override string ToString()
    {
        return "Note de "+Valeur +" pour l'étudiant "+Etudiant.Nom+" "+Etudiant.Prenom+" en "+Ue.Intitule;
    }
}