using System.Globalization;
using System.Text;
using TUnit.Assertions;

namespace Air.Domain.Fares.Test.Shared.Asserters;

public class AssertExBuilder
{
    //Act Action
    //Assert.Fail (needs action)
    //Assert needs thrown exception
    //Exception catch

    //AssertExBuilder.Act(() => act())
    //.AssertException<T>(ex => asserts(ex))
    public AssertExBuilder(Action act)
    {
        _act = act;
    }

    private Action? _act;
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
        catch (Exception e)
        {
            if (typeof(T) == e.GetType())
            {
                assert((T)e);
                return;
            }
            throw new UnexpectedExceptionThrownException($"An unexpected exception '{e.GetType().Name}' was thrown, see inner exception, message: '{e.Message}', stack trace: {e.StackTrace}", e);
        }
    }
}

public static class AssertEx
{
    public static void EnsureExceptionMessageContains(Exception exception, params string[] expectedMessageParts)
    {
        var exceptionMessages = new StringBuilder();
        _ = exceptionMessages.Append(CultureInfo.InvariantCulture, $"An Exception of type {exception.GetType()} was thrown, however the following message parts were not found in the exception message.");
        _ = exceptionMessages.AppendLine(CultureInfo.InvariantCulture, $"The Actual Exception Message is: {exception.Message}");

        var somePartNotFound = false;

        foreach (var part in expectedMessageParts)
        {
            if (!exception.Message.Contains(part))
            {
                somePartNotFound = true;
                _ = exceptionMessages.AppendLine(CultureInfo.InvariantCulture, $"The Expected substring '{part}' was not found in the Exception Message.");
            }
        }

        if (somePartNotFound)
        {
            throw new FailedExceptionAssertionException(exceptionMessages + "\nStack trace:\n" + exception.StackTrace, exception);
        }
    }

    public static void EnsureExceptionMessageDoesNotContains(Exception exception, params string[] expectedMessageParts)
    {
        var exceptionMessages = new StringBuilder();
        _ = exceptionMessages.Append(CultureInfo.InvariantCulture, $"An Exception of type {exception.GetType()} was thrown, however the message contains parts that were Not Expected");
        _ = exceptionMessages.AppendLine(CultureInfo.InvariantCulture, $"The Actual Exception Message is: {exception.Message}");

        var somePartFound = false;

        foreach (var part in expectedMessageParts)
        {
            if (exception.Message.Contains(part))
            {
                somePartFound = true;
                _ = exceptionMessages.AppendLine(CultureInfo.InvariantCulture, $"The substring '{part}' was Not Expected to be contained in the Exception Message.");
            }
        }

        if (somePartFound)
        {
            throw new FailedExceptionAssertionException(exceptionMessages + "\nStack trace:\n" + exception.StackTrace, exception);
        }
    }
}
