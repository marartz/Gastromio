﻿using Gastromio.Core.Domain.Model.Cuisines;

namespace Gastromio.Core.Application.Commands.RemoveCuisine
{
    public class RemoveCuisineCommand : ICommand
    {
        public RemoveCuisineCommand(CuisineId cuisineId)
        {
            CuisineId = cuisineId;
        }

        public CuisineId CuisineId { get; }
    }
}
