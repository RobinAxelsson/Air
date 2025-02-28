// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain.Fares.Exceptions
{
    [ExcludeFromCodeCoverage]
    public abstract class AirFaresTechnicalBaseException : AirFaresBaseException
    {
        protected AirFaresTechnicalBaseException()
        {
        }

        protected AirFaresTechnicalBaseException(string message)
            : base(message)
        {
        }

        protected AirFaresTechnicalBaseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
