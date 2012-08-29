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
        private List<GhostJoint> _savedJoints;

        public List<GhostJoint> SavedJoints
        {
            get
            {
                _savedJoints = _savedJoints ?? new List<GhostJoint>();
                return _savedJoints;
            }
            private set
            {
                _savedJoints = value;
            }
        }

        public GhostSkeleton(Skeleton skeleton) : this()
        {
            _savedJoints = new List<GhostJoint>();
            foreach (Joint joint in skeleton.Joints)
            {
                GhostJoint newJoint = new GhostJoint(joint);
                this.GhostJoints.Add(newJoint);
                _savedJoints.Add(newJoint);
            }
        }

        partial void OnLoaded()
        {
            //Console.WriteLine("Loading skeleton with ID :" + this.ID);
            this.UpdateCache();
        }

        public GhostJoint GetJoint(JointType type)
        {
            GhostJoint ghostJoint = this.SavedJoints.SingleOrDefault(joint => joint.JointType == ((int)type));
            return ghostJoint;
        }

        public void UpdateCache()
        {
            this.SavedJoints = this.GhostJoints.ToList();
        }
    }
}
