﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Ghosts
{
    public partial class GhostSkeleton
    {
        public List<GhostJoint> SavedJoints
        {
            get;
            private set;
        }

        public GhostSkeleton(Skeleton skeleton) : this()
        {
            foreach (Joint joint in skeleton.Joints)
            {
                this.GhostJoints.Add(new GhostJoint(joint));
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
