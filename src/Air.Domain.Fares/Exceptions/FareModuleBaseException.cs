// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain.Fares.Exceptions
{
    [ExcludeFromCodeCoverage]
    public abstract class FareModuleBaseException : Exception
    {
        public abstract string Reason { get; }

        protected FareModuleBaseException()
        {
        }

        protected FareModuleBaseException(string message)
            : base(message)
        {
        }

        protected FareModuleBaseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
