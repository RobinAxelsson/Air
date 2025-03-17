// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain.Fares.Test.Acceptance.TestExceptions;

internal abstract class AirTestException : Exception
{
    protected AirTestException()
    {
    }

    protected AirTestException(string? message) : base(message)
    {
    }

    protected AirTestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

internal class InvalidTestCloneException : AirTestException
{
    public InvalidTestCloneException()
    {
    }

    public InvalidTestCloneException(string? message) : base(message)
    {
    }

    public InvalidTestCloneException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

internal class FailedExceptionAssertionException : AirTestException
{
    public FailedExceptionAssertionException()
    {
    }

    public FailedExceptionAssertionException(string? message) : base(message)
    {
    }

    public FailedExceptionAssertionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

internal class MoreThenOneTestAccessAspEnvironment : AirTestException
{
    public MoreThenOneTestAccessAspEnvironment()
    {
    }

    public MoreThenOneTestAccessAspEnvironment(string? message) : base(message)
    {
    }

    public MoreThenOneTestAccessAspEnvironment(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
