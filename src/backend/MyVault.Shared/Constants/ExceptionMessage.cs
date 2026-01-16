using System;

namespace MyVault.Shared.Constants;

public static class ExceptionMessage
{
    public static string CONFIGURATION_PROPERTY_NOT_FOUND(string propertyName) => $"{propertyName} not found";
    public const string INCORRECT_FORMAT = "incorrect format";
    public static string NOT_EXISTS(string element) => $"{element} not exists";
}
