using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Ghosts
{
    public partial class GhostSkeletonSequence
    {
        public static int DefaultInterpolationFactor = 2;

        public int CurrentFrame
        {
            get;
            set;
        }

        public TimeSpan Length
        {
            get
            {
                if (this.EndDate.HasValue || this.StartDate.HasValue)
                {
                    return TimeSpan.Zero;
                }

                return (this.EndDate.Value - this.StartDate.Value);
            }
        }

        public List<GhostSkeleton> SavedSkeletons
        {
            get;
            private set;
        }

        partial void OnLoaded()
        {
            Console.WriteLine("Loading sequence with ID: " + this.ID);
            this.UpdateCache(DefaultInterpolationFactor);
        }

        public void AddSkeleton(Skeleton skeleton)
        {
            if (this.GhostSkeletons.Count == 0)
            {
                this.StartDate = DateTime.Now;
                this.EndDate = DateTime.MaxValue;
            }

            GhostSkeleton ghostSkeleton = new GhostSkeleton(skeleton);
            this.GhostSkeletons.Add(ghostSkeleton);
        }

        public void FinalizeRecording()
        {
            this.EndDate = DateTime.Now;
        }

        public void UpdateCache(int interpolationFactor = 0)
        {
            for (int i = 0; i < this.GhostSkeletons.Count - 1; i += (interpolationFactor + 1))
            {
                GhostSkeleton current = this.GhostSkeletons[i];
                GhostSkeleton next = this.GhostSkeletons[i + 1];

                List<GhostSkeleton> newSkeletons = new List<GhostSkeleton>();
                for (int j = 0; j < interpolationFactor; j++)
                {
                    GhostSkeleton newSkeleton = new GhostSkeleton();
                    this.GhostSkeletons.Insert(i + j + 1, newSkeleton);
                    newSkeletons.Add(newSkeleton);
                }

                foreach (GhostJoint joint in current.GhostJoints)
                {
                    GhostJoint nextJoint = next.GetJoint((JointType)joint.JointType);
                    if (nextJoint == null)
                    {
                        continue;
                    }

                    GhostJoint deltaJoint = (nextJoint - joint) / (interpolationFactor + 1);
                    for (int j = 0; j < interpolationFactor; j++)
                    {
                        GhostJoint newJoint = joint + (deltaJoint * (j + 1));
                        newSkeletons[j].GhostJoints.Add(newJoint);
                    }
                }
            }

            this.SavedSkeletons = this.GhostSkeletons.ToList();
            this.SavedSkeletons.ForEach(skel => skel.UpdateCache());
        }
    }
}
