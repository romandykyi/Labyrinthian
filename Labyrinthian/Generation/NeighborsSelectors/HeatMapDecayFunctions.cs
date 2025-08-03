using System;
using static System.Math;

namespace Labyrinthian
{
    /// <summary>
    /// Provides various decay functions for adjusting heat map values.
    /// </summary>
    public static class HeatMapDecayFunctions
    {
        /// <summary>
        /// Create a linear decay function for the heat map values.
        /// </summary>
        /// <param name="delta">Amount to decrement the temperature by each time.</param>
        /// <returns>A function that performs linear decay on a <see cref="HeatMapValue"/>.</returns>
        public static Func<HeatMapValue, double> Linear(double delta = 0.2)
        {
            return x => Max(x.Temperature - delta, 0.0);
        }

        /// <summary>
        /// Create an exponential decay function for the heat map values.
        /// </summary>
        /// <param name="alpha">Decay constant for exponential decay (must be positive or 0).</param>
        /// <returns>A function that performs exponential decay on a <see cref="HeatMapValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="alpha"/> is not positive.</exception>
        public static Func<HeatMapValue, double> Exponential(double alpha)
        {
            if (alpha < 0.0)
                throw new ArgumentOutOfRangeException(nameof(alpha), alpha, "alpha must be positive or 0.");

            return x => x.Temperature * Exp(-alpha);
        }

        /// <summary>
        /// Create a multiplicative decay function for the heat map values.
        /// </summary>
        /// <param name="alpha">Decay factor for multiplicative decay (must be within the range [0, 1]).</param>
        /// <returns>A function that performs multiplicative decay on a <see cref="HeatMapValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="alpha"/> is outside the valid range.</exception>
        public static Func<HeatMapValue, double> Multiplicative(double alpha = 0.05)
        {
            if (alpha < 0.0 || alpha > 1.0)
                throw new ArgumentOutOfRangeException(nameof(alpha), alpha, "alpha must be within the range from 0 to 1.");

            return x => x.Temperature * alpha;
        }

        /// <summary>
        /// Create an inverse visit count decay function for the heat map values.
        /// </summary>
        /// <returns>A function that performs inverse visit count decay on a <see cref="HeatMapValue"/>.</returns>
        public static Func<HeatMapValue, double> InverseVisit()
        {
            return x => 1.0 / (x.VisitsCount + 2.0);
        }

        /// <summary>
        /// Create a logarithmic decay function for the heat map values.
        /// </summary>
        /// <param name="logBase">Base of the logarithm (must be greater than 1).</param>
        /// <returns>A function that performs logarithmic decay on a <see cref="HeatMapValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="logBase"/> is not greater than 1.</exception>
        public static Func<HeatMapValue, double> Logarithmic(double logBase = 2)
        {
            if (logBase <= 1)
                throw new ArgumentOutOfRangeException(nameof(logBase), logBase, "logBase must be greater than 1.");

            return x => 1.0 / (Log(x.VisitsCount + 1, logBase) + logBase);
        }
    }
}
