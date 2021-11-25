﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeCuisine
{
    public class ChangeCuisineCommandHandler : ICommandHandler<ChangeCuisineCommand>
    {
        private readonly ICuisineRepository cuisineRepository;

        public ChangeCuisineCommandHandler(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }

        public async Task HandleAsync(ChangeCuisineCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var cuisine = await cuisineRepository.FindByCuisineIdAsync(command.CuisineId, cancellationToken);
            if (cuisine == null)
                throw DomainException.CreateFrom(new CuisineDoesNotExistFailure());

            cuisine.ChangeName(command.Name, currentUser.Id);

            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
        }
    }
}
