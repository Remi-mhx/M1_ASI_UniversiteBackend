using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface INoteRepository : IRepository<Note>
{
    Task<Note> CreateNoteAsync(Note note);
    Task<Note> CreateNoteAsync(long idEtudiant, long idUe, float note);
    Task<Note> CreateNoteAsync(Etudiant etudiant, Ue ue, float note);
    
}