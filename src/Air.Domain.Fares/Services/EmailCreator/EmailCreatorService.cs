// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Air.Domain.Fares.Services.EmailCreator.Dtos;
using Air.Domain.Fares.Services.EmailCreator.Mappers;
using Air.Domain.Fares.Services.EmailCreator.ViewModels;
using RazorLight;

namespace Air.Domain.Fares.Services.EmailCreator
{
    internal static class EmailCreatorService
    {
        internal static async Task<string> CreateWeekendEmail(WeekendSurfViewModelDto viewModelDto)
        {
            var razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailCreatorService).Assembly)
                .SetOperatingAssembly(typeof(EmailCreatorService).Assembly)
                .UseMemoryCachingProvider()
                .Build();

            //var assembly = Assembly.GetExecutingAssembly();
            //var resourceName = "Air.Domain.Fares.Services.EmailCreator.Templates.WeekendSurfTemplate";

            //using var stream = assembly.GetManifestResourceStream(resourceName);
            //using var reader = new StreamReader(stream);
            //var template = await reader.ReadToEndAsync();



            return await razorEngine.CompileRenderAsync("Air.Domain.Fares.Services.EmailCreator.Templates.WeekendSurfTemplate", ViewModelMapper.MapToViewModel(viewModelDto));
        }
    }
}
