using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<Note> ExecuteAsync(float valeur, long idEtudiant, long idUe)
    {
        var note = new Note{Valeur = valeur, IdEtudiant = idEtudiant, IdUe = idUe};
        return await ExecuteAsync(note);
    }
    public async Task<Note> ExecuteAsync(Note note)
    {
        await CheckBusinessRules(note);
        Note n = await repositoryFactory.NoteRepository().CreateAsync(note);
        repositoryFactory.NoteRepository().SaveChangesAsync().Wait();
        return n;
    }
    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.Valeur);
        ArgumentNullException.ThrowIfNull(note.IdEtudiant);
        ArgumentNullException.ThrowIfNull(note.IdUe);
        
        // Un étudiant n'a qu'une note par Ue
        List<Note> existe = await repositoryFactory.NoteRepository().FindByConditionAsync
            (e=>e.IdEtudiant.Equals(note.IdEtudiant) && e.IdUe.Equals(note.IdUe));
        
        // Si une note pour cet étudiant et cette UE existe déjà, on lève une exception personnalisée
        if (existe is {Count:>0}) throw new DuplicateNoteException("Une note pour cet étudiant et cette UE existe déjà");
        
        // La note doit être comprise entre 0 et 20
        if (note.Valeur < 0 || note.Valeur > 20) throw new ValeurNoteException("La note doit être comprise entre 0 et 20");
        
        // Un étudiant ne peut avoir une note que dans une Ue du parcours dans lequel il est inscrit
        Ue ue = await repositoryFactory.UeRepository().FindAsync(note.IdUe) ?? throw new InvalidOperationException("L'UE n'existe pas");
        Etudiant etudiant = await repositoryFactory.EtudiantRepository().FindAsync(note.IdEtudiant) ?? throw new InvalidOperationException("L'étudiant n'existe pas");
        if(!etudiant.ParcoursSuivi.UesEnseignees.Contains(ue)) throw new UeNonInscriteException("L'étudiant n'est pas inscrit à cette UE");
    }
}