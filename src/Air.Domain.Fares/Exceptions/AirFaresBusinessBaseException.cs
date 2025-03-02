// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public abstract class AirFaresBusinessBaseException : AirFaresBaseException
{
    protected AirFaresBusinessBaseException()
    {
    }

    protected AirFaresBusinessBaseException(string message)
        : base(message)
    {
    }

    protected AirFaresBusinessBaseException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
