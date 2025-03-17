// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Air.Domain.Fares.Test.Acceptance.TestExceptions;

namespace Air.Domain.Fares.Test.Acceptance.TestDataGenerators.Helpers;
internal static class TripSpecDtoCloneValidator
{
    //No validation for types - that does the compiler
    public static void EnsureCloneIsIdentical()
    {
        Type originalType = typeof(FlightSpecDto);
        Type cloneType = typeof(FlightSpecDtoClone);

        string? classNameError = ValidateClassName(originalType, cloneType);
        string? propertyNamesError = ValidatePropertyNames(originalType, cloneType);

        var errors = classNameError + propertyNamesError;

        if (errors.Length != 0)
        {
            throw new InvalidTestCloneException($"The test is initializing test data for type '{originalType.Name}' using a test clone type '{cloneType.Name}', test clones need to have the exact same properties as the originals.\n" + errors);
        }
    }

    private static string? ValidatePropertyNames(Type originalType, Type cloneType)
    {
        var originalProperties = originalType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Select(p => p.Name).ToArray();
        var cloneProperties = cloneType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Select(p => p.Name);

        var missingProperties = originalProperties.Except(cloneProperties);
        var missingPropertiesError = missingProperties.Count() != 0 ? $"Missing properties in clone '{string.Join(", ", missingProperties)}'\n" : null;

        var extraProperties = cloneProperties.Except(originalProperties);
        var extraPropertiesError = extraProperties.Count() != 0 ? $"Too many properties on {cloneType.Name}, remove or rename '{extraProperties}'\n" : null;

        return missingPropertiesError + extraPropertiesError;
    }

    private static string? ValidateClassName(Type originalType, Type cloneType)
    {
        return cloneType.Name != originalType.Name + "Clone" ? $"Class names does not match, clone class name should be '{originalType.Name + "Clone"}' not '{cloneType.Name}'\n" : null;
    }
}
