using FoodOrderSystem.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodOrderSystem.Domain.Model
{
    public class FailureResultDTO
    {
        // it has to be named 'Errors' to match [FromBody] model validation.
        // this way messages from both sources can be handled uniformly on clients.
        public IDictionary<string, IList<string>> Errors { get; }

        public FailureResultDTO(IDictionary<string, IList<string>> errors)
        {
            Errors = errors;
        }
    }

}
