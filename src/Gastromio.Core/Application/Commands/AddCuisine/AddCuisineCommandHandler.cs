﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.AddCuisine
{
    public class AddCuisineCommandHandler : ICommandHandler<AddCuisineCommand, CuisineDTO>
    {
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;

        public AddCuisineCommandHandler(
            ICuisineFactory cuisineFactory,
            ICuisineRepository cuisineRepository
        )
        {
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<CuisineDTO> HandleAsync(AddCuisineCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            if (string.IsNullOrWhiteSpace(command.Name))
                throw DomainException.CreateFrom(new CuisineNameIsRequiredFailure());

            var cuisine = await cuisineRepository.FindByNameAsync(command.Name, cancellationToken);
            if (cuisine != null)
                throw DomainException.CreateFrom(new CuisineAlreadyExistsFailure());

            cuisine = cuisineFactory.Create(command.Name, currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            return new CuisineDTO(cuisine);
        }
    }
}
