using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.SecurityUseCases.Create;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository(UniversiteDbContext context) : Repository<Note>(context), INoteRepository
{
    public async Task<Note> CreateNoteAsync(Note note)
    {
        return await CreateNoteAsync(note.EtudiantId, note.UeId, note.Valeur);
    }
    
    public async Task<Note> CreateNoteAsync(Etudiant etudiant, Ue ue, float note)
    {
        return await CreateNoteAsync(etudiant.Id, ue.Id, note);
    }
    
    public async Task<Note> CreateNoteAsync(long idEtudiant, long idUe, float note)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        ArgumentNullException.ThrowIfNull(Context.Notes);
        Etudiant etudiant = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        Note n = new Note {Valeur = note, Etudiant = etudiant, Ue = ue};
        await Context.AddAsync(n);
        await Context.SaveChangesAsync();
        return n;
    }

    
    public async Task<double> GetNoteAsync(long idEtudiant, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return (await Context.Notes.FirstOrDefaultAsync(n => n.Etudiant.Id == idEtudiant && n.Ue.Id == idUe))?.Valeur ?? 0;
    }
    
    public async Task<double> GetNoteAsync(Etudiant etudiant, Ue ue)
    {
        return await GetNoteAsync(etudiant.Id, ue.Id);
    }
    public async Task<IEnumerable<Note>> GetNotesAsync(Etudiant etudiant)
    {
        return await GetNotesAsync(etudiant.Id);
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes.Where(n => n.Ue.Id == idUe).ToListAsync();
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(Ue ue)
    {
        return await GetNotesAsync(ue.Id);
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(long idEtudiant, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        return await Context.Notes.Where(n => n.Etudiant.Id == idEtudiant && n.Ue.Id == idUe).ToListAsync();
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(Etudiant etudiant, Ue ue)
    {
        return await GetNotesAsync(etudiant.Id, ue.Id);
    }
}