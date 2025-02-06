using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Note> ExecuteAsync(float valeur, long idEtudiant, long idUe)
    {
        var note = new Note
        {
            Valeur = valeur,
            Etudiant = new Etudiant {Id = idEtudiant},
            Ue = new Ue {Id = idUe}
        };
        return await ExecuteAsync(note);
    }

    public async Task<Note> ExecuteAsync(Note note)
    {
        Note newNote = await repositoryFactory.NoteRepository().CreateAsync(note);
        await CheckBusinessRules(newNote);
        repositoryFactory.NoteRepository().SaveChangesAsync().Wait();
        return newNote;
    }

    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.Valeur);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(note.Valeur);
        ArgumentNullException.ThrowIfNull(note.Etudiant);
        ArgumentNullException.ThrowIfNull(note.Ue);
        ArgumentNullException.ThrowIfNull(repositoryFactory.NoteRepository());
        
        if (note.Valeur < 0 || note.Valeur > 20)
        {
            throw new OutRangeNoteException("La note doit être comprise entre 0 et 20");
        }
        
        List<Note> noteList = await repositoryFactory.NoteRepository().FindByConditionAsync(p => p.Etudiant.Id == note.Etudiant.Id && p.Ue.Id == note.Ue.Id);
        if (noteList is {Count:>0})
        {
            throw new DuplicateNoteException("Une note pour cet étudiant et cette UE existe déjà");
        }
        
        List<Parcours> parcours = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.Inscrits.Any(e => e.Id == note.Etudiant.Id) && p.UesEnseignees.Any(u => u.Id == note.Ue.Id));
        if(parcours is {Count:0})
        {
            throw new ParcoursNotFoundException("L'étudiant n'est pas inscrit dans cette UE");
        }
    }
    
}