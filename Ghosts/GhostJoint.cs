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
