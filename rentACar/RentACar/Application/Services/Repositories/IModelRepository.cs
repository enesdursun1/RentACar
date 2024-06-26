﻿using Domain.Entities;
using Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IModelRepository : IAsyncRepository<Model, Guid>, IRepository<Model, Guid>
{

}
