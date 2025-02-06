using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NoteUseCases.Create;

namespace UniversiteDomainUnitTests;

public class NoteUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task CreateNoteUseCase()
    {
        long id = 1;
        float valeur = 15;
        long idEtudiant = 1;
        long idUe = 1;

        // On crée la note qui doit être ajoutée en base
        Note noteSansId = new Note { Valeur = valeur, IdEtudiant = idEtudiant, IdUe = idUe };

        //  Créons le mock du repository
        // On initialise une fausse datasource qui va simuler un NoteRepository
        var mockNote = new Mock<INoteRepository>();

        Ue ue = new Ue { Id = 1, NumeroUe = "UE1", Intitule = "UE1" };
        var mockUeRepository = new Mock<IUeRepository>();
        mockUeRepository.Setup(repo => repo.FindAsync(It.IsAny<long>())).ReturnsAsync(ue);

        Parcours parcours = new Parcours { Id = 1, NomParcours = "Parcours1", UesEnseignees = new List<Ue>{ue}};
        Etudiant etudiant = new Etudiant{Id= 1, Nom = "Nom", Prenom = "Prenom", Email = "toto@gmail.com",
            ParcoursSuivi = parcours};

        var mockEtudiantRepository = new Mock<IEtudiantRepository>();
        mockEtudiantRepository.Setup(repo => repo.FindAsync(It.IsAny<long>())).ReturnsAsync(etudiant);

        // Il faut ensuite aller dans le use case pour voir quelles fonctions simuler
        // Nous devons simuler FindByCondition et Create

        // Simulation de la fonction FindByCondition
        // On dit à ce mock que la note n'existe pas déjà
        // La réponse à l'appel FindByCondition est donc une liste vide
        var reponseFindByCondition = new List<Note>();
        // On crée un bouchon dans le mock pour la fonction FindByCondition
        // Quelque soit le paramètre de la fonction FindByCondition, on renvoie la liste vide
        mockNote.Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Note, bool>>>()))
            .ReturnsAsync(reponseFindByCondition);

        // Simulation de la fonction Create
        // On lui dit que l'ajout d'une note renvoie une note avec l'Id 1
        Note noteCree = new Note { Valeur = valeur, IdEtudiant = idEtudiant, IdUe = idUe };
        mockNote.Setup(repoNote => repoNote.CreateAsync(noteSansId)).ReturnsAsync(noteCree);

        // On crée le bouchon (un faux noteRepository). Il est prêt à être utilisé
        var fauxNoteRepository = mockNote.Object;

        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto => facto.NoteRepository()).Returns(fauxNoteRepository);
        mockFactory.Setup(facto => facto.UeRepository()).Returns(mockUeRepository.Object);
        mockFactory.Setup(facto => facto.EtudiantRepository()).Returns(mockEtudiantRepository.Object);

        // Création du use case en injectant notre faux repository
        CreateNoteUseCase useCase = new CreateNoteUseCase(mockFactory.Object);
        // / Appel du use case
        var noteTeste = await useCase.ExecuteAsync(noteSansId);

        // Vérification du résultat
        Assert.That(noteTeste.Valeur, Is.EqualTo(noteCree.Valeur));
        Assert.That(noteTeste.IdEtudiant, Is.EqualTo(noteCree.IdEtudiant));
        Assert.That(noteTeste.IdUe, Is.EqualTo(noteCree.IdUe));
    }

}