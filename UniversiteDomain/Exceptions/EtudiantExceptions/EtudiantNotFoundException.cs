namespace UniversiteDomain.Exceptions.EtudiantExceptions;

[Serializable]
public class EtudiantNotFoundException : Exception
{
    public EtudiantNotFoundException() : base() { }
    public EtudiantNotFoundException(string message) : base("L'étudiant avec l'identifiant " + message + " n'existe pas.") { }
    public EtudiantNotFoundException(string message, Exception inner) : base(message, inner) { }
}