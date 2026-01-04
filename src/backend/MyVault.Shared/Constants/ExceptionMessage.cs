using System;

namespace MyVault.Shared.Constants;

public static class ExceptionMessage
{
    public static string CONFIGURATION_PROPERTY_NOT_FOUND(string propertyName) => $"{propertyName} not found";
    public static string INCORRECT_FORMAT = "incorrect format";
}
