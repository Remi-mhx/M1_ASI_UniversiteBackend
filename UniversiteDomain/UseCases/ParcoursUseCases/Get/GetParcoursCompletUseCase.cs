using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Get;

public class GetParcoursCompletUseCase(IRepositoryFactory factory)
{
    public async Task<Parcours> ExecuteAsync(long idParcours)
    {
        await CheckBusinessRules();
        Parcours? parcours = await factory.ParcoursRepository().FindParcoursCompletAsync(idParcours);
        if (parcours == null) throw new ParcoursNotFoundException();
        return parcours;
    }
    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IParcoursRepository parcoursRepository=factory.ParcoursRepository();
        ArgumentNullException.ThrowIfNull(parcoursRepository);
    }
    public bool IsAuthorized(string role, IUniversiteUser user, long idParcours)
    {
        return role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable);
    }
    
}