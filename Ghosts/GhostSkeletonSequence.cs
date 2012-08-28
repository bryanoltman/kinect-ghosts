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

        //partial void OnCreated()
        //{
        //    Console.WriteLine("sequence created with ID: " + this.ID);
        //    this.SavedSkeletons = this.GhostSkeletons.ToList();
        //}

        partial void OnLoaded()
        {
            Console.WriteLine("Loading sequence with ID: " + this.ID);
            this.UpdateCache();
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

        public void UpdateCache()
        {
            this.SavedSkeletons = this.GhostSkeletons.ToList();
            this.SavedSkeletons.ForEach(skel => skel.UpdateCache());
        }
    }
}
