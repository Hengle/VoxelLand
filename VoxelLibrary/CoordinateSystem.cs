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
            get { return new CoordinateSystem(Point.Origin, Vector.UnitX, Vector.UnitY, Vector.UnitZ); }
        }

        public CoordinateSystem()
        {
            rotation = Matrix.Identity;
            translation = Vector.Zero;
        }

        public CoordinateSystem(Point origin, Vector xAxis, Vector yAxis, Vector zAxis)
        {
            rotation = new Matrix(new float[]
                {
                    xAxis.X,  xAxis.Y,  xAxis.Z,  0,
                    yAxis.X,  yAxis.Y,  yAxis.Z,  0,
                    zAxis.X,  zAxis.Y,  zAxis.Z,  0,
                    0,        0,        0,        1
                });

            translation = origin - Point.Origin;
        }

        public override bool Equals(object obj)
        {
            if (! (obj is CoordinateSystem))
                return false;

            CoordinateSystem other = (CoordinateSystem)obj;

            return rotation.Equals(other.rotation) && translation.Equals(other.translation);
        }

        public Vector ToGlobal(Vector v)
        {
            return rotation * v;
        }

        public Point ToGlobal(Point p)
        {
            return (rotation * p) + translation;
        }

        public Vector ToLocal(Vector v)
        {
            return rotation.Transposed() * v;
        }

        public Point ToLocal(Point p)
        {
            return rotation.Transposed() * (p - translation);
        }

        public CoordinateSystem LocallyTranslated(Vector v)
        {
            return new CoordinateSystem(rotation, translation + (rotation * v));
        }

        public CoordinateSystem GloballyTranslated(Vector v)
        {
            return new CoordinateSystem(rotation, translation + v);
        }

        public CoordinateSystem LocallyRotated(float angle, Vector axis)
        {
            return new CoordinateSystem(Transform.Rotate(angle, axis) * rotation, translation);
        }

        public CoordinateSystem GloballyRotated(float angle, Vector axis)
        {
            return new CoordinateSystem(Transform.Rotate(angle, rotation.Transposed() * axis) * rotation, Transform.Rotate(angle, axis) * translation);
        }

        public override string ToString()
        {
            return String.Format("{0} + {1}", rotation, translation);
        }

        private CoordinateSystem(Matrix rotation, Vector translation)
        {
            this.rotation = rotation;
            this.translation = translation;
        }

        Matrix rotation;
        Vector translation;
    }
}
