using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Ghosts
{
    public partial class GhostJoint
    {
        public static GhostJoint operator /(GhostJoint joint, int denominator)
        {
            return new GhostJoint()
            {
                JointType = joint.JointType,
                X = joint.X / denominator,
                Y = joint.Y / denominator,
                Z = joint.Z / denominator
            };
        }

        public static GhostJoint operator *(GhostJoint joint, float f)
        {
            return new GhostJoint()
            {
                JointType = joint.JointType,
                X = joint.X * f,
                Y = joint.Y * f,
                Z = joint.Z * f
            };
        }

        public static GhostJoint operator +(GhostJoint joint0, GhostJoint joint1)
        {
            if (joint0.JointType != joint1.JointType)
            {
                throw new Exception("Joints must be of the same type to be added");
            }

            GhostJoint newJoint = new GhostJoint();
            newJoint.JointType = joint0.JointType;
            newJoint.X = (joint0.X + joint1.X);
            newJoint.Y = (joint0.Y + joint1.Y);
            newJoint.Z = (joint0.Z + joint1.Z);

            return newJoint;
        }

        public static GhostJoint operator -(GhostJoint joint0, GhostJoint joint1)
        {
            if (joint0.JointType != joint1.JointType)
            {
                throw new Exception("Joints must be of the same type to be subtracted");
            }

            GhostJoint newJoint = new GhostJoint();
            newJoint.JointType = joint0.JointType;
            newJoint.X = (joint0.X - joint1.X);
            newJoint.Y = (joint0.Y - joint1.Y);
            newJoint.Z = (joint0.Z - joint1.Z);

            return newJoint;
        }

        public override string ToString()
        {
            return "X:" + this.X + " Y:" + this.Y + " Z:" + this.Z;
        }

        public GhostJoint(Joint joint)
            : this()
        {
            X = joint.Position.X;
            Y = joint.Position.Y;
            Z = joint.Position.Z;
            JointType = (int)joint.JointType;
        }

    }
}
