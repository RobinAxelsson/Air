// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.Services.EmailCreator.Dtos;
using Air.Domain.Fares.Services.EmailCreator.ViewModels;

namespace Air.Domain.Fares.Services.EmailCreator.Mappers
{
    internal class ViewModelMapper
    {
        public static WeekendSurfViewModel MapToViewModel(WeekendSurfViewModelDto dto)
        {
            return new WeekendSurfViewModel
            {
                Origin = dto.Origin.ToString(),
                Destination = dto.Destination.ToString(),
                Price = dto.Price,
                Airline = dto.Airline.ToString(),
                DepartureDate = dto.DepartureDate
            };
        }
    }
}
