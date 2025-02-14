﻿using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Get;

public class GetAllParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    public async Task<List<Parcours>> ExecuteAsync()
    {
        List<Parcours> parcours = await repositoryFactory.ParcoursRepository().FindAllAsync();
        return parcours;
    }

    private async Task CheckBusinessRules(List<Parcours> parcours)
    {
    }
}