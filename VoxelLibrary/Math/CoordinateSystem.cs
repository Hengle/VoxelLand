using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public class CoordinateSystem
    {
        public static CoordinateSystem Default
        {
            get { return new CoordinateSystem(); }
        }

        public CoordinateSystem()
        {
            Rotation = Quaternion.Identity;
            Origin = Point.Origin;
        }

        public Quaternion Rotation { get; set; }
        public Point      Origin   { get; set; }

        public override bool Equals(object obj)
        {
            if (! (obj is CoordinateSystem))
                return false;

            CoordinateSystem other = (CoordinateSystem)obj;

            return Rotation.Equals(other.Rotation) && Origin.Equals(other.Origin);
        }

        public override string ToString()
        {
            return String.Format("{0} + {1}", Rotation, Origin);
        }

        #region Matrix Conversions
        public Matrix ModelMatrix
        {
            get { return Transform.Translate(Origin - Point.Origin) * Rotation; }
        }

        public Matrix ViewMatrix
        {
            get { return Rotation.Conjugate() * Transform.Translate(Point.Origin - Origin); }
        }
        #endregion

        #region Move Between Coordinate System
        public Vector ToGlobal(Vector v)
        {
            return Rotation * v;
        }

        public Vector ToLocal(Vector v)
        {
            return Rotation.Conjugate() * v;
        }

        public Point ToGlobal(Point p)
        {
            return Origin + (Rotation * (p - Point.Origin));
        }

        public Point ToLocal(Point p)
        {
            return Point.Origin + (Rotation.Conjugate() * (p - Origin));
        }

        public CoordinateSystem ToGlobal(CoordinateSystem c)
        {
            return new CoordinateSystem(Rotation * c.Rotation, ToGlobal(c.Origin));
        }

        public CoordinateSystem ToLocal(CoordinateSystem c)
        {
            return new CoordinateSystem(c.Rotation * Rotation.Conjugate(), ToLocal(c.Origin));
        }
        #endregion

        #region Transformations
        // Rotate(angle, axis, point)
        public CoordinateSystem LocallyTranslated(Vector v)
        {
            return new CoordinateSystem(Rotation, Origin + (Rotation * v));
        }

        public CoordinateSystem GloballyTranslated(Vector v)
        {
            return new CoordinateSystem(Rotation, Origin + v);
        }

        public CoordinateSystem LocallyRotated(float angle, Vector axis)
        {
            return LocallyRotated(Transform.RotateQ(angle, axis));
        }

        public CoordinateSystem LocallyRotated(float pitch, float yaw, float roll)
        {
            return LocallyRotated(Transform.Rotate(pitch, yaw, roll));
        }

        public CoordinateSystem GloballyRotated(float angle, Vector axis)
        {
            return GloballyRotated(Transform.RotateQ(angle, axis));
        }

        public CoordinateSystem GloballyRotated(float pitch, float yaw, float roll)
        {
            return GloballyRotated(Transform.Rotate(pitch, yaw, roll));
        }

        private CoordinateSystem LocallyRotated(Quaternion newRotation)
        {
            return new CoordinateSystem(Rotation * newRotation, Origin);
        }

        private CoordinateSystem GloballyRotated(Quaternion newRotation)
        {
            return new CoordinateSystem(newRotation * Rotation, newRotation * Origin);
        }
        #endregion

        private CoordinateSystem(Quaternion rotation, Point origin)
        {
            Rotation = rotation;
            Origin = origin;
        }
    }
}
