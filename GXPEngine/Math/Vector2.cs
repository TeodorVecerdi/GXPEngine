using System;
using System.Globalization;

namespace GXPEngine {
    /// <summary>
    ///     Representation of 2D vectors and points.
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>, IFormattable {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     X component of the vector
        /// </summary>
        public float x;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     Y component of the vector
        /// </summary>
        public float y;

        public Vector2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///     Sets the x and y components of this vector
        /// </summary>
        /// <param name="x">The new x component</param>
        /// <param name="y">The new y component</param>
        public void Set(float x, float y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///     Scales the current vector by the vector <paramref name="scale" />
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector2 scale) {
            x *= scale.x;
            y *= scale.y;
        }

        /// <summary>
        ///     Normalizes the current vector
        /// </summary>
        public void Normalize() {
            var mag = magnitude;
            if (mag > float.Epsilon)
                this = this / mag;
            else
                this = zero;
        }

        /// <summary>
        ///     Gets the normalized vector
        /// </summary>
        public Vector2 normalized {
            get {
                var v = new Vector2(x, y);
                v.Normalize();
                return v;
            }
        }
        
        /// <summary>
        ///     Returns the length of this vector
        /// </summary>
        public float magnitude => Mathf.Sqrt(x * x + y * y);

        /// <summary>
        ///     Returns the squared length of this vector
        /// </summary>
        public float sqrMagnitude => x * x + y * y;

        /// <summary>
        ///     Sets the angle of the current vector
        /// </summary>
        /// <param name="degrees">Degrees</param>
        public void SetAngleDegrees(float degrees) {
            SetAngleRadians(Mathf.Deg2Rad * degrees);
        }

        /// <summary>
        ///     Sets the angle of the current vector
        /// </summary>
        /// <param name="radians">Radians</param>
        public void SetAngleRadians(float radians) {
            var m = magnitude;
            var unit = GetUnitVectorRad(radians);
            Set(unit.x * m, unit.y * m);
        }

        /// <summary>
        ///     Returns the angle of the current vector in degrees
        /// </summary>
        /// <returns>The angle of the current vector in degrees</returns>
        public float GetAngleDegrees() {
            return Mathf.Deg2Rad * GetAngleRadians();
        }

        /// <summary>
        ///     Returns the angle of the current vector in radians
        /// </summary>
        /// <returns>The angle of the current vector in radians</returns>
        public float GetAngleRadians() {
            var n = normalized;
            var angle = Mathf.Atan2(n.y, n.x);
            return angle;
        }

        /// <summary>
        ///     Rotates the current vector by <paramref name="degrees" /> degrees.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        public void RotateDegrees(float degrees) {
            RotateRadians(Mathf.Deg2Rad * degrees);
        }

        /// <summary>
        ///     Rotates the current vector by <paramref name="radians" /> radians.
        /// </summary>
        /// <param name="radians">Radians</param>
        public void RotateRadians(float radians) {
            var c = Mathf.Cos(radians);
            var s = Mathf.Sin(radians);
            var newX = x * c - y * s;
            var newY = x * s + y * c;
            Set(newX, newY);
        }

        /// <summary>
        ///     Rotates this Vec2 around <paramref name="point" /> by <paramref name="degrees" />
        /// </summary>
        /// <param name="point">The point around which to rotate</param>
        /// <param name="degrees">The amount of degrees to rotate by</param>
        public void RotateAroundDegrees(Vector2 point, float degrees) {
            RotateAroundRadians(point, degrees * Mathf.Deg2Rad);
        }

        /// <summary>
        ///     Rotates this Vec2 around <paramref name="point" /> by <paramref name="radians" />
        /// </summary>
        /// <param name="point">The point around which to rotate</param>
        /// <param name="radians">The amount of radians to rotate by</param>
        public void RotateAroundRadians(Vector2 point, float radians) {
            var copy = this;
            copy -= point;
            copy.RotateRadians(radians);
            copy += point;
            Set(copy.x, copy.y);
        }

        public override bool Equals(object obj) {
            return obj is Vector2 other && Equals(other);
        }

        public bool Equals(Vector2 other) {
            return Math.Abs(x - other.x) < float.Epsilon && Math.Abs(y - other.y) < float.Epsilon;
        }

        public override int GetHashCode() {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        /// <summary>
        ///     Returns a nicely formatted string for the vector
        /// </summary>
        public override string ToString() {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format))
                format = "F1";
            return string.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }
        
        /// <summary>
        ///     Returns a unit vector rotated by <paramref name="degrees" /> degrees
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>Unit vector rotated by <paramref name="degrees" /> degrees</returns>
        public static Vector2 GetUnitVectorDeg(float degrees) {
            return GetUnitVectorRad(Mathf.Deg2Rad * degrees);
        }

        /// <summary>
        ///     Returns a unit vector rotated by <paramref name="radians" /> radians
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>Unit vector rotated by <paramref name="radians" /> radians</returns>
        public static Vector2 GetUnitVectorRad(float radians) {
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        /// <summary>
        ///     Returns a <see cref="GXPEngine.Vector2" /> perpendicular on the <paramref name="inDirection" /> parameter
        /// </summary>
        public static Vector2 Perpendicular(Vector2 inDirection) {
            return new Vector2(-inDirection.y, inDirection.x);
        }

        /// <summary>
        ///     Converts a <see cref="GXPEngine.Vector3" /> to a Vector2.
        /// </summary>
        public static implicit operator Vector2(Vector3 v) {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        ///     Converts a Vector2 to a <see cref="GXPEngine.Vector3" />.
        /// </summary>
        public static implicit operator Vector3(Vector2 v) {
            return new Vector3(v.x, v.y, 0);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b) {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b) {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        public static Vector2 operator -(Vector2 a) {
            return new Vector2(-a.x, -a.y);
        }

        public static Vector2 operator *(Vector2 a, float d) {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator *(float d, Vector2 a) {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator /(Vector2 a, float d) {
            return new Vector2(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs) {
            var diffX = lhs.x - rhs.x;
            var diffY = lhs.y - rhs.y;
            return diffX * diffX + diffY * diffY < Mathf.Epsilon;
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs) {
            return !(lhs == rhs);
        }

        public static Vector2 zero { get; } = new Vector2(0f, 0f);
        public static Vector2 one { get; } = new Vector2(1f, 1f);
        public static Vector2 up { get; } = new Vector2(0f, 1f);
        public static Vector2 down { get; } = new Vector2(0f, -1f);
        public static Vector2 left { get; } = new Vector2(-1f, 0f);
        public static Vector2 right { get; } = new Vector2(1f, 0f);
        public static Vector2 positiveInfinity { get; } = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        public static Vector2 negativeInfinity { get; } = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
    }
}