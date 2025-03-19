// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using TUnit.Assertions;

namespace Air.Domain.Fares.Test.Shared.Asserters;

//The TUnit library does not seem to have a way to assert on Exception messages so we us the AssertEx helpers to give better messages
public class AssertExBuilder
{
    private AssertExBuilder(Action act)
    {
        _act = act;
    }

    private readonly Action? _act;
    public static AssertExBuilder Act(Action act)
    {
        return new AssertExBuilder(act);
    }

    public void AssertThrows<T>(Action<T> assert) where T : Exception
    {
        if(_act == null) throw new NullReferenceException("This should never happen");

        try
        {
            _act();
            Assert.Fail($"We were expecting an {typeof(T).Name} exception to be thrown but no exception was thrown");
        }
        catch(T e)
        {
            assert((T)e);
        }
        catch (Exception e)
        {
            throw new TestExceptions.UnexpectedExceptionThrownException($"An unexpected exception '{e.GetType().Name}' was thrown, see inner exception, message: '{e.Message}', stack trace: {e.StackTrace}", e);
        }
    }
}
