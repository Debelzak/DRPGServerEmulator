
namespace DRPGServer.Common
{
    public readonly struct Vector2 : IEquatable<Vector2>
    {
        public int X { get; }
        public int Y { get; }

        public static readonly Vector2 Zero = new(0, 0);
        public static readonly Vector2 One = new(1, 1);
        public static readonly Vector2 Up = new(0, -1);
        public static readonly Vector2 Down = new(0, 1);
        public static readonly Vector2 Left = new(-1, 0);
        public static readonly Vector2 Right = new(1, 0);

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Soma
        public Vector2 Add(Vector2 other) => new(X + other.X, Y + other.Y);

        // Subtração
        public Vector2 Subtract(Vector2 other) => new(X - other.X, Y - other.Y);

        public int DistanceSquared(Vector2 other)
        {
            int dx = X - other.X;
            int dy = Y - other.Y;
            return dx * dx + dy * dy;
        }

        public double Distance(Vector2 other)
        {
            return Math.Sqrt(DistanceSquared(other));
        }

        // Igualdade
        public bool Equals(Vector2 other) => X == other.X && Y == other.Y;

        public override bool Equals(object? obj) => obj is Vector2 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);
        public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);

        public static Vector2 operator +(Vector2 a, Vector2 b) => a.Add(b);
        public static Vector2 operator -(Vector2 a, Vector2 b) => a.Subtract(b);

        public override string ToString() => $"({X}, {Y})";
    }
}
