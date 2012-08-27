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
        public int currentFrame
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
    }
}
