// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.Models.atomic;

namespace Air.Domain;

internal static class CurrencyParser
{
    private static HashSet<Currency> _currencySet = new HashSet<Currency>(Enum.GetValues<Currency>());
    private readonly static string _currencyCodes = String.Join(", ", Enum.GetNames(typeof(Currency)));
    public static Currency ParseCurrencyCode(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new InvalidCurrencyException("Currency was null or empty string");
        }

        currencyCode = currencyCode.Trim().ToUpper();

        if (!Enum.TryParse(currencyCode, out Currency result))
        {
            throw new InvalidCurrencyException($"Currency code '{currencyCode}' is not valid. Valid codes are: {_currencyCodes}");
        }

        return result;
    }

    public static string? ValidationMessage(Currency currency)
    {
        return !_currencySet.Contains(currency) ? $"The Currency: {currency} is not a valid Currency. Valid values are: {_currencyCodes}" : null;
    }
}
