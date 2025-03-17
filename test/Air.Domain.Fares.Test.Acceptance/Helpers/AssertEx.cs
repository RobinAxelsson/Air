using System.Globalization;
using System.Text;
using Air.Domain.Fares.Test.Acceptance.TestExceptions;

namespace Air.Domain.Fares.Test.Acceptance.Helpers;

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
            throw new FailedExceptionAssertionException(exceptionMessages.ToString(), exception);
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
            throw new FailedExceptionAssertionException(exceptionMessages.ToString(), exception);
        }
    }
}
