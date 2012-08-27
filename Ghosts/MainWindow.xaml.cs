using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace Ghosts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields
        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        private KinectSensor sensor;

        private GhostDataContext dataContext;

        private Dictionary<int, GhostSkeletonSequence> trackingIDsToSequences = new Dictionary<int, GhostSkeletonSequence>();

        private List<GhostSkeletonSequence> activeSequences = new List<GhostSkeletonSequence>();

        private object lockObj = new object();

        private int frame = 0;

        private bool isSaving = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Private Methods

        private void DrawBonesAndJoints(GhostSkeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton.GetJoint(JointType.Head), skeleton.GetJoint(JointType.ShoulderCenter), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderCenter), skeleton.GetJoint(JointType.ShoulderLeft), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderCenter), skeleton.GetJoint(JointType.ShoulderRight), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderCenter), skeleton.GetJoint(JointType.Spine), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.Spine), skeleton.GetJoint(JointType.HipCenter), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.HipCenter), skeleton.GetJoint(JointType.HipLeft), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.HipCenter), skeleton.GetJoint(JointType.HipRight), drawingContext);

            // Left Arm
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderLeft), skeleton.GetJoint(JointType.ElbowLeft), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.ElbowLeft), skeleton.GetJoint(JointType.WristLeft), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.WristLeft), skeleton.GetJoint(JointType.HandLeft), drawingContext);

            // Right Arm
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderRight), skeleton.GetJoint(JointType.ElbowRight), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.ElbowRight), skeleton.GetJoint(JointType.WristRight), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.WristRight), skeleton.GetJoint(JointType.HandRight), drawingContext);

            // Left Leg
            this.DrawBone(skeleton.GetJoint(JointType.HipLeft), skeleton.GetJoint(JointType.KneeLeft), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.KneeLeft), skeleton.GetJoint(JointType.AnkleLeft), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.AnkleLeft), skeleton.GetJoint(JointType.FootLeft), drawingContext);

            // Right Leg
            this.DrawBone(skeleton.GetJoint(JointType.HipRight), skeleton.GetJoint(JointType.KneeRight), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.KneeRight), skeleton.GetJoint(JointType.AnkleRight), drawingContext);
            this.DrawBone(skeleton.GetJoint(JointType.AnkleRight), skeleton.GetJoint(JointType.FootRight), drawingContext);
        }

        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton.Joints[JointType.Head], skeleton.Joints[JointType.ShoulderCenter], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderLeft], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderRight], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.Spine], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.Spine], skeleton.Joints[JointType.HipCenter], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipLeft], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipRight], drawingContext);

            // Left Arm
            this.DrawBone(skeleton.Joints[JointType.ShoulderLeft], skeleton.Joints[JointType.ElbowLeft], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.ElbowLeft], skeleton.Joints[JointType.WristLeft], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.WristLeft], skeleton.Joints[JointType.HandLeft], drawingContext);

            // Right Arm
            this.DrawBone(skeleton.Joints[JointType.ShoulderRight], skeleton.Joints[JointType.ElbowRight], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.WristRight], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.WristRight], skeleton.Joints[JointType.HandRight], drawingContext);

            // Left Leg
            this.DrawBone(skeleton.Joints[JointType.HipLeft], skeleton.Joints[JointType.KneeLeft], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.AnkleLeft], skeleton.Joints[JointType.FootLeft], drawingContext);

            // Right Leg
            this.DrawBone(skeleton.Joints[JointType.HipRight], skeleton.Joints[JointType.KneeRight], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight], drawingContext);
            this.DrawBone(skeleton.Joints[JointType.AnkleRight], skeleton.Joints[JointType.FootRight], drawingContext);

            //// Render Joints
            //foreach (Joint joint in skeleton.Joints)
            //{
            //    Brush drawBrush = null;

            //    if (joint.TrackingState == JointTrackingState.Tracked)
            //    {
            //        drawBrush = this.trackedJointBrush;
            //    }
            //    else if (joint.TrackingState == JointTrackingState.Inferred)
            //    {
            //        drawBrush = this.inferredJointBrush;
            //    }

            //    if (drawBrush != null)
            //    {
            //        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
            //    }
            //}
        }

        private void DrawBone(Joint joint0, Joint joint1, DrawingContext drawingContext)
        {
            this.DrawBone(new GhostJoint(joint0), new GhostJoint(joint1), drawingContext);
        }

        private void DrawBone(GhostJoint joint0, GhostJoint joint1, DrawingContext drawingContext)
        {
            //Joint joint0 = skeleton.Joints[jointType0];
            //Joint joint1 = skeleton.Joints[jointType1];

            //// If we can't find either of these joints, exit
            //if (joint0.TrackingState == JointTrackingState.NotTracked ||
            //    joint1.TrackingState == JointTrackingState.NotTracked)
            //{
            //    return;
            //}

            //// Don't draw if both points are inferred
            //if (joint0.TrackingState == JointTrackingState.Inferred &&
            //    joint1.TrackingState == JointTrackingState.Inferred)
            //{
            //    return;
            //}

            //// We assume all drawn bones are inferred unless BOTH joints are tracked
            //Pen drawPen = this.inferredBonePen;
            //if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            //{
            //    drawPen = this.trackedBonePen;
            //}

            Pen drawPen = this.trackedBonePen;
            SkeletonPoint point0 = new SkeletonPoint() { X = joint0.X, Y = joint0.Y, Z = joint0.Z };
            SkeletonPoint point1 = new SkeletonPoint() { X = joint1.X, Y = joint1.Y, Z = joint1.Z };

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(point0), this.SkeletonPointToScreen(point1));
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.MapSkeletonPointToDepth(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
        #endregion

        #region Event Handlers
        private void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            frame++;

            Skeleton[] skeletons = new Skeleton[0];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (frame % 2 == 0)
                {
                    lock (lockObj)
                    {
                        List<GhostSkeletonSequence> toRemove = new List<GhostSkeletonSequence>();
                        foreach (GhostSkeletonSequence sequence in activeSequences)
                        {
                            if (sequence.currentFrame >= sequence.GhostSkeletons.Count - 1)
                            {
                                sequence.currentFrame = 0;
                                toRemove.Add(sequence);
                                continue;
                            }

                            GhostSkeleton skeleton = sequence.GhostSkeletons[sequence.currentFrame];
                            this.DrawBonesAndJoints(skeleton, dc);
                            sequence.currentFrame++;
                        }

                        toRemove.ForEach(sequence => activeSequences.Remove(sequence));
                    }
                }

                // Used to determine when skeletons move off the screen
                List<int> unseenTrackingIDs = this.trackingIDsToSequences.Keys.ToList();

                foreach (Skeleton skel in skeletons)
                {
                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (!isSaving)
                        {
                            GhostSkeletonSequence sequence;

                            if (!this.trackingIDsToSequences.ContainsKey(skel.TrackingId))
                            {
                                Console.WriteLine("Adding tracking ID " + skel.TrackingId);
                                sequence = new GhostSkeletonSequence();
                                lock (lockObj)
                                {
                                    this.dataContext.GhostSkeletonSequences.InsertOnSubmit(sequence);
                                }

                                this.trackingIDsToSequences.Add(skel.TrackingId, sequence);
                            }
                            else
                            {
                                sequence = this.trackingIDsToSequences[skel.TrackingId];
                            }

                            unseenTrackingIDs.Remove(skel.TrackingId);

                            sequence.AddSkeleton(skel);
                        }

                        this.DrawBonesAndJoints(skel, dc);
                    }
                }

                foreach (int trackingID in unseenTrackingIDs)
                {
                    Console.WriteLine("Disposing of " + trackingID);
                    GhostSkeletonSequence sequence = this.trackingIDsToSequences[trackingID];
                    sequence.FinalizeRecording();
                    Console.WriteLine("Finalizing sequence " + sequence.ID);
                    this.trackingIDsToSequences.Remove(trackingID);
                }

                if (unseenTrackingIDs.Count != 0)
                {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += (obj, args) =>
                        {
                            lock (lockObj)
                            {
                                isSaving = true;
                                Console.WriteLine("beginning save");
                                dataContext.SubmitChanges();
                                Console.WriteLine("saved!");
                                isSaving = false;
                            }
                        };

                        worker.RunWorkerAsync();
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isSaving)
            {
                return;
            }

            GhostSkeletonSequence sequence;
            lock (lockObj)
            {
                if (this.dataContext.GhostSkeletonSequences.Count() == 0)
                {
                    return;
                }

                sequence = (from row in this.dataContext.GhostSkeletonSequences
                            orderby this.dataContext.Random()
                            select row).FirstOrDefault();
                Console.WriteLine("Ticked! Adding sequence with id " + sequence.ID);
                this.activeSequences.Add(sequence);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dataContext = App.DataContext;

            //this.dataContext.DeleteDatabase();
            //App.Current.Shutdown();

            if (!this.dataContext.DatabaseExists())
            {
                try
                {
                    this.dataContext.CreateDatabase();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("EXCEPTION");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Deleting...");
                    try
                    {
                        this.dataContext.DeleteDatabase();
                        Console.WriteLine("Database deleted!");
                    }
                    catch (SqlException ex2)
                    {
                        Console.WriteLine("Error deleting DB");
                        Console.WriteLine(ex2.Message);
                    }

                    App.Current.Shutdown();
                }
            }

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            this.SkeletonImage.Source = this.imageSource;
            foreach (KinectSensor potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (this.sensor != null)
            {
                this.sensor.SkeletonStream.Enable();
                this.sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;

                try
                {
                    Console.WriteLine("Starting the sensor");
                    this.sensor.Start();
                    Console.WriteLine("Sensor started");

                    Timer timer = new Timer(5000);
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Sensor start failed. " + ex.Message);
                    this.sensor = null;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sensor != null)
            {
                this.sensor.Stop();
            }
        }
        #endregion
    }
}
