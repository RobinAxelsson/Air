// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public abstract class AirFaresBaseException : Exception
{
    public abstract string Reason { get; }

    protected AirFaresBaseException()
    {
    }

    protected AirFaresBaseException(string message)
        : base(message)
    {
    }

    protected AirFaresBaseException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public AirFaresBaseException(string message, object serializable, Exception inner): base(message, inner)
    {

    }
}
