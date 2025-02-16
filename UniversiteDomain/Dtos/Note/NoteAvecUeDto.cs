using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class NoteAvecUeDto
{
    public UeDto UeDto{get; set;}
    public float Valeur { get; set; }

    public NoteAvecUeDto ToDto(Note note)
    {
        UeDto = new UeDto().ToDto(note.Ue);
        Valeur = note.Valeur;
        return this;
    }

    
    public static List<NoteAvecUeDto> ToDtos(List<Note> notes)
    {
        List<NoteAvecUeDto> dtos = new();
        foreach (var note in notes)
        {
            dtos.Add(new NoteAvecUeDto().ToDto(note));
        }
        return dtos;
    }

}