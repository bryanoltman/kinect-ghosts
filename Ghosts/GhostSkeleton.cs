using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Ghosts
{
    public partial class GhostSkeleton
    {
        public GhostSkeleton(Skeleton skeleton) : this()
        {
            foreach (Joint joint in skeleton.Joints)
            {
                this.GhostJoints.Add(new GhostJoint(joint));
            }
        }

        public GhostJoint GetJoint(JointType type)
        {
            GhostJoint ghostJoint = this.GhostJoints.SingleOrDefault(joint => joint.JointType == ((int)type));
            return ghostJoint;
        }
    }
}
