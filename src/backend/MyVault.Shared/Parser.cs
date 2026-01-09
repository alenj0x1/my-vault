using System;

namespace MyVault.Shared;

public static class Parser
{
    public static bool EnumToString<T>(string value, out T result) where T : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase: true, out result);
    }
}
