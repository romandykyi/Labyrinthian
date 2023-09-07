using System;

namespace Labyrinthian
{
    /// <summary>
    /// Class for patterns used for mazes on 2D grid.
    /// </summary>
    public static class Grid2DPatterns
    {
        /// <summary>
        /// Predicate for the rectangular pattern with inner rectangular room.
        /// </summary>
        /// <param name="width">Width of the main rectangle. Should be greater than 1.</param>
        /// <param name="height">Height of the main rectangle. Should be greater than 1.</param>
        /// <param name="inWidth">
        /// Width of the inner rectangle. Should be positive and not greater than
        /// <c><paramref name="width"/> - 2</c>
        /// </param>
        /// <param name="inHeight">
        /// Height of the inner rectangle. Should be positive and not greater than
        /// <c><paramref name="height"/> - 2</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Predicate<GridPoint2D> RectangularPattern(int width, int height, int inWidth = 0, int inHeight = 0)
        {
            if (width <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (inWidth < 0 || inWidth > width - 2)
            {
                throw new ArgumentOutOfRangeException(nameof(inWidth));
            }
            if (inHeight < 0 || inHeight > height - 2)
            {
                throw new ArgumentOutOfRangeException(nameof(inHeight));
            }

            int m0 = (height - inHeight) / 2;
            int m1 = m0 + inHeight;
            int n0 = (width - inWidth) / 2;
            int n1 = n0 + inWidth;

            return p => p.Row < m0 || p.Row >= m1 ||
                p.Column < n0 || p.Column >= n1;
        }

        /// <summary>
        /// Predicate for the triangular pattern with inner triangular room.
        /// </summary>
        /// <param name="sideLength">Side length of the triangle. Should be greater than 1.</param>
        /// <param name="inSideLength">
        /// Side length of the inner triangle. Should be positive and not greater than 
        /// <c><paramref name="sideLength"/> - 3</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Predicate<GridPoint2D> TriangularPattern(int sideLength, int inSideLength = 0)
        {
            if (sideLength <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(sideLength));
            }
            if (inSideLength < 0 || inSideLength > sideLength - 3)
            {
                throw new ArgumentOutOfRangeException(nameof(inSideLength));
            }

            int k = sideLength - 1;
            int r = inSideLength - 1;

            int mid = (sideLength - inSideLength) / 2;
            mid += mid % 2;
            int maxI = mid + inSideLength;

            int q = sideLength - inSideLength;

            return p =>
                Math.Abs(p.Column - k) <= p.Row &&
                (Math.Abs(p.Column - r - q) > p.Row - mid || p.Row >= maxI);
        }

        /// <summary>
        /// Predicate for the hexagonal pattern with inner hexagonal room.
        /// </summary>
        /// <param name="sideLength">Side length of the hexagon. Should be greater than 1.</param>
        /// <param name="inSideLength">
        /// Side length of the inner hexagon. Should be positive and not greater than 
        /// <c><paramref name="sideLength"/> - 1</c>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Predicate<GridPoint2D> HexagonalPattern(int sideLength, int inSideLength)
        {
            if (sideLength <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(sideLength));
            }
            if (inSideLength < 0 || inSideLength > sideLength - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(inSideLength));
            }

            int n = 2 * sideLength - 1;
            int m = 2 * inSideLength - 1;

            int q = (n - m) / 2;

            return p =>
            {
                int k = Math.Abs(sideLength - 1 - p.Column);
                int inI = p.Row - q;
                int inJ = p.Column - q;
                int r = Math.Abs(inSideLength - 1 - inJ);

                return
                    p.Row >= k / 2 && p.Row < n - k / 2 - k % 2 &&
                    (inI < 0 || inI >= m || inJ < 0 || inJ >= m ||
                    inI < r / 2 || inI >= m - r / 2 - r % 2);
            };
        }

        /// <summary>
        /// Predicate for the hexagonal pattern with inner hexagonal room on the triangular grid.
        /// </summary>
        /// <param name="sideLength">Side length of the hexagon. Should be greater than 1.</param>
        /// <param name="inSideLength">
        /// Side length of the inner hexagon. Should be positive and not greater than 
        /// <c><paramref name="sideLength"/> - 1</c>
        /// </param>
        public static Predicate<GridPoint2D> DeltaHexagonalPattern(int sideLength, int inSideLength)
        {
            int r = (sideLength + 1) % 2;
            Predicate<GridPoint2D> pattern = HexagonalPattern(sideLength, inSideLength);

            return (p) =>
            {
                int row = p.Row, col = p.Column;

                col /= 3;
                int d = (col + r) % 2;
                if (d == 1 && row == 0) return false;
                else row = (row - d) / 2;

                return pattern(new GridPoint2D(row, col));
            };
        }

        /// <summary>
        /// Predicate for the circular pattern with inner circular room.
        /// </summary>
        /// <param name="radius">Radius of the base circle. Should be greater than 1.</param>
        /// <param name="inRadius">
        /// Radius of the inner circle. Should be positive and not greater than 
        /// <c><paramref name="radius"/> - 1</c>
        /// </param>
        public static Predicate<GridPoint2D> CirclePredicate(int radius, int inRadius)
        {
            if (radius <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }
            if (inRadius < 0 || inRadius > radius - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(inRadius));
            }

            return p => p.Row >= inRadius;
        }
    }
}
