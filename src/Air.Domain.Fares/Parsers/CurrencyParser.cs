// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain;

namespace Air.Domain;

internal static class CurrencyParser
{
    private static HashSet<Currency> _currencySet = new HashSet<Currency>(Enum.GetValues<Currency>()[1..]);

    private static string CurrencyCodes() => String.Join(", ", _currencySet);
    public static Currency ParseCurrencyCode(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new InvalidCurrencyException($"Currency was null or empty string, valid codes are: {CurrencyCodes()}");
        }

        currencyCode = currencyCode.Trim().ToUpper();

        if (!Enum.TryParse(currencyCode, out Currency result))
        {
            throw new InvalidCurrencyException($"Currency code '{currencyCode}' is not valid. Valid codes are: {CurrencyCodes()}");
        }

        return result;
    }

    public static string? ValidationMessage(Currency currency)
    {
        return !_currencySet.Contains(currency) ? $"The Currency: {currency} is not a valid Currency. Valid values are: {CurrencyCodes()}" : null;
    }
}
