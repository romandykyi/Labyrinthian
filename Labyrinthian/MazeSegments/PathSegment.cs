using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Labyrinthian
{
    public abstract class PathSegment
    {
        public abstract Vector2 StartPoint { get; }
        public abstract Vector2 EndPoint { get; }
        public abstract float[] Center { get; }

        protected Vector2 Transform(Vector2 v, float cellSize, float offset)
        {
            return new Vector2(v.X * cellSize + offset, v.Y * cellSize + offset);
        }

        protected virtual string MoveToStartSvg(Vector2 v) =>
            $"M {v.X.ToInvariantString()} {v.Y.ToInvariantString()} ";

        protected abstract string MoveToEndSvg(Vector2 v);

        public string MoveToStartSvg(float cellSize, float offset)
        {
            return MoveToStartSvg(Transform(StartPoint, cellSize, offset));
        }
        public string MoveToEndSvg(float cellSize, float offset)
        {
            return MoveToEndSvg(Transform(EndPoint, cellSize, offset));
        }

        public string MoveNext(PathSegment? from, float cellSize, float offset)
        {
            string result = "";
            if (from == null || !IsContinuous(from, this))
            {
                result += MoveToStartSvg(cellSize, offset);
            }
            result += MoveToEndSvg(cellSize, offset);
            return result;
        }

        public string FromStartToEnd(float cellSize, float offset)
        {
            string start = MoveToStartSvg(cellSize, offset),
                end = MoveToEndSvg(cellSize, offset);

            return $"{start}{end}";
        }

        public static bool IsContinuous(PathSegment first, PathSegment last)
        {
            Vector2 v1 = first.EndPoint, v2 = last.StartPoint;
            return v1.X.ApproximatelyEquals(v2.X) && v1.Y.ApproximatelyEquals(v2.Y);
        }
    }
}