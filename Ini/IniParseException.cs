using System;

namespace Rampastring.Tools.Ini;

public class IniParseException : Exception
{
    public IniParseException(string message) : base(message)
    {
    }
}
