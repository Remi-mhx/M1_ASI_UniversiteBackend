﻿using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Get;

public class GetAllEtudiantsUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<List<Etudiant>> ExecuteAsync()
    {
        List<Etudiant> etudiants = await repositoryFactory.EtudiantRepository().FindAllAsync();
        await CheckBusinessRules(etudiants);
        return etudiants;
    }

    private async Task CheckBusinessRules(List<Etudiant> etudiants)
    {
    }
}