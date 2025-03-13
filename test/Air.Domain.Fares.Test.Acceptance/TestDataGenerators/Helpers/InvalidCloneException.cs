// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain.Fares.Test.Acceptance.Helpers
{
    internal class InvalidCloneException : Exception
    {
        public InvalidCloneException()
        {
        }

        public InvalidCloneException(string? message) : base(message)
        {
        }

        public InvalidCloneException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
