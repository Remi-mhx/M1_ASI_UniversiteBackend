namespace UniversiteDomain.Exceptions.NoteExceptions;

public class OutRangeNoteException : Exception
{
    public OutRangeNoteException() : base() { }
    public OutRangeNoteException(string message) : base(message) { }
    public OutRangeNoteException(string message, Exception inner) : base(message, inner) { }
}