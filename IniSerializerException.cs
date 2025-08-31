using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rampastring.Tools;

public class IniSerializerException : Exception
{
    public IniSerializerException(string message) : base(message)
    {
    }
}
