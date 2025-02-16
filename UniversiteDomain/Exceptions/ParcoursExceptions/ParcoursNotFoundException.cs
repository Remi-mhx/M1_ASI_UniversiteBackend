namespace UniversiteDomain.Exceptions.ParcoursExceptions;

[Serializable]
public class ParcoursNotFoundException : Exception
{
    public ParcoursNotFoundException() : base() { }
    public ParcoursNotFoundException(string message) : base("Le parcours avec l'identifiant " + message + " n'existe pas.") { }
    public ParcoursNotFoundException(string message, Exception inner) : base(message, inner) { }
}