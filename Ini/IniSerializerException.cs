using System;

namespace Rampastring.Tools.Ini;

public class IniSerializerException : Exception
{
    public IniSerializerException(string message) : base(message)
    {
    }
}
