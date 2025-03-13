// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;


namespace Air.Domain.Fares.Test.Acceptance.Helpers;
internal static class TripSpecDtoCloneValidator
{
    public static void EnsureCloneIsIdentical()
    {
        Type originalType = typeof(FlightSpecDto);
        Type cloneType = typeof(FlightSpecDtoClone);

        string? classNameError = ValidateClassName(originalType, cloneType);
        string? propertyNamesError = ValidatePropertyNames(originalType, cloneType);
        var propertyTypesError = ValidatePropertyTypes(originalType, cloneType);

        var errors = classNameError + propertyNamesError + propertyTypesError;

        if (errors.Length != 0)
        {
            throw new InvalidCloneException($"propType2 '{cloneType.Name}' does not match its propType1 class '{originalType.FullName}'\n" + errors);
        }
    }

    private static string ValidatePropertyTypes(Type originalType, Type cloneType)
    {
        (Type propertyType, string propertyName)[] GetPropertyTypeAndName(Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Select(p => (p.PropertyType, p.Name)).ToArray();
        var originalPropertiesTypeNames = GetPropertyTypeAndName (originalType);

        var clonePropertyTypeNames = GetPropertyTypeAndName (cloneType);

        var originalNames = originalPropertiesTypeNames.Select(x => x.propertyName);
        var cloneNames = clonePropertyTypeNames.Select(x => x.propertyName);
        var matchingPropertyNames = originalNames.Intersect(cloneNames);

        var sb = StringBuilderCache.Acquire();
        foreach (var propertyName in matchingPropertyNames)
        {
            var originalPropertyType = originalPropertiesTypeNames.Single(x => x.propertyName == propertyName).propertyType;
            var clonePropertyType = clonePropertyTypeNames.Single(x => x.propertyName == propertyName).propertyType;

            if (!TypeEqualsAllowNullable(originalPropertyType, clonePropertyType))
            {
                sb.AppendLine($"{originalType.Name}.{propertyName} has type '{originalPropertyType.Name}' not '{clonePropertyType.Name}'\n");
            }
        }

        return sb.ToString();
    }

    private static bool TypeEqualsAllowNullable(Type propType1, Type propType2)
    {
        //normal type .FullName example
        //FullName  "Air.Domain.AirportCode"}

        //nullable type .FullName example
        //FullName	"System.Nullable`1[[Air.Domain.AirportCode, Air.Domain.Fares, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"	string

        var originalName = propType1.FullName?.Split(',')[0].Split('[')[^1];
        var cloneName = propType2.FullName?.Split(',')[0].Split('[')[^1];

        if (originalName == null || cloneName == null) throw new ArgumentNullException("should not be possible");
        return originalName == cloneName;
    }

    private static string? ValidatePropertyNames(Type originalType, Type cloneType)
    {
        var originalProperties = originalType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Select(p => p.Name).ToArray();
        var cloneProperties = cloneType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Select(p => p.Name);

        var missingProperties = originalProperties.Except(cloneProperties);
        var missingPropertiesError = missingProperties.Count() != 0 ? $"Missing properties in propType2 '{string.Join(", ", missingProperties)}'\n" : null;

        var extraProperties = cloneProperties.Except(originalProperties);
        var extraPropertiesError = extraProperties.Count() != 0 ? $"Too many properties on propType2, remove or rename '{extraProperties}'\n" : null;

        return missingPropertiesError + extraPropertiesError;
    }

    private static string? ValidateClassName(Type originalType, Type cloneType) => cloneType.Name != originalType.Name + "Clone" ? $"Class names does not match, propType2 class name should be '{originalType + "Clone"}' not '{cloneType.Name}'\n" : null;
}
