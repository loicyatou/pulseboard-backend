using System;

namespace PulseBoard.Services.Extensions;

public static class EnumExtensions
{
    public static string ToEnumName<TEnum>(this int value)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
            throw new ArgumentException($"Value '{value}' is not valid for enum {typeof(TEnum).Name}");

        return Enum.GetName(typeof(TEnum), value)!;
    }
}
