using System;

namespace Labyrinthian
{
    /// <summary>
    /// Exception that replaces <see cref="ArgumentNullException"/> for <see cref="SvgFill"/>.
    /// </summary>
    [Serializable]
    public sealed class SvgFillNullException : Exception
    {
        /// <summary>
        /// Constructor with a default message that uses <paramref name="variableName"/>
        /// </summary>
        /// <param name="variableName">Name of the variable which is <see langword="null"/>.</param>
        public SvgFillNullException(string variableName) :
            base($"{variableName} is null. SvgFill cannot be null, if you want not to fill object please use SvgFill.None instead")
        { }
    }
}
