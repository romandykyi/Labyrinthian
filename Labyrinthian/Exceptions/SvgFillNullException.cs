using System;

namespace Labyrinthian
{
    public sealed class SvgFillNullException : Exception
    {
        public SvgFillNullException(string variableName) :
            base($"{variableName} is null. SvgFill cannot be null, if you want not to fill object please use SvgFill.None instead") { }
    }
}
