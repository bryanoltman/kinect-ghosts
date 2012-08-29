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
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        private KinectSensor sensor;

        private Dictionary<int, GhostSkeletonSequence> trackingIDsToSequences = new Dictionary<int, GhostSkeletonSequence>();

        private List<GhostSkeletonSequence> savedSequences;

        private List<GhostSkeletonSequence> activeSequences = new List<GhostSkeletonSequence>();

        private object lockObj = new object();

        private Random random = new Random();

        private bool isSaving = false;

        private int frameCount = 0;

        private GhostDataContext dataContext;

        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Intermediate storage for the color data received from the camera
        /// </summary>
        private byte[] colorPixels;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Private Methods

        private void DrawBonesAndJoints(GhostSkeleton skeleton, DrawingContext drawingContext)
        {
            byte alpha = 100;

            // Render Torso
            this.DrawBone(skeleton.GetJoint(JointType.Head), skeleton.GetJoint(JointType.ShoulderCenter), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderCenter), skeleton.GetJoint(JointType.ShoulderLeft), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderCenter), skeleton.GetJoint(JointType.ShoulderRight), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderCenter), skeleton.GetJoint(JointType.Spine), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.Spine), skeleton.GetJoint(JointType.HipCenter), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.HipCenter), skeleton.GetJoint(JointType.HipLeft), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.HipCenter), skeleton.GetJoint(JointType.HipRight), drawingContext, alpha);

            // Left Arm
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderLeft), skeleton.GetJoint(JointType.ElbowLeft), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.ElbowLeft), skeleton.GetJoint(JointType.WristLeft), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.WristLeft), skeleton.GetJoint(JointType.HandLeft), drawingContext, alpha);

            // Right Arm
            this.DrawBone(skeleton.GetJoint(JointType.ShoulderRight), skeleton.GetJoint(JointType.ElbowRight), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.ElbowRight), skeleton.GetJoint(JointType.WristRight), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.WristRight), skeleton.GetJoint(JointType.HandRight), drawingContext, alpha);

            // Left Leg
            this.DrawBone(skeleton.GetJoint(JointType.HipLeft), skeleton.GetJoint(JointType.KneeLeft), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.KneeLeft), skeleton.GetJoint(JointType.AnkleLeft), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.AnkleLeft), skeleton.GetJoint(JointType.FootLeft), drawingContext, alpha);

            // Right Leg
            this.DrawBone(skeleton.GetJoint(JointType.HipRight), skeleton.GetJoint(JointType.KneeRight), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.KneeRight), skeleton.GetJoint(JointType.AnkleRight), drawingContext, alpha);
            this.DrawBone(skeleton.GetJoint(JointType.AnkleRight), skeleton.GetJoint(JointType.FootRight), drawingContext, alpha);
        }

        private void DrawBone(Joint joint0, Joint joint1, DrawingContext drawingContext, byte alpha)
        {
            this.DrawBone(new GhostJoint(joint0), new GhostJoint(joint1), drawingContext, alpha);
        }

        private void DrawBone(GhostJoint joint0, GhostJoint joint1, DrawingContext drawingContext, byte alpha)
        {
            Pen drawPen = new Pen(new SolidColorBrush(Color.FromArgb(alpha, 0, 0, 0)), 6);
            Point point0 = this.SkeletonPointToScreen(new SkeletonPoint() { X = joint0.X, Y = joint0.Y, Z = joint0.Z });
            Point point1 = this.SkeletonPointToScreen(new SkeletonPoint() { X = joint1.X, Y = joint1.Y, Z = joint1.Z });

            int padding = 3;
            if (point0.X < padding || point0.Y < padding || point0.X + padding > RenderWidth || point0.Y + padding > RenderHeight ||
                point1.X < padding || point1.Y < padding || point1.X + padding > RenderWidth || point1.Y + padding > RenderHeight)
            {
                return;
            }

            drawingContext.DrawLine(drawPen, point0, point1);
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dataContext = new GhostDataContext();

            //this.dataContext.DeleteDatabase();
            //App.Current.Shutdown();

            if (!dataContext.DatabaseExists())
            {
                try
                {
                    dataContext.CreateDatabase();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("EXCEPTION");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Deleting...");
                    try
                    {
                        dataContext.DeleteDatabase();
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

            this.savedSequences = dataContext.GhostSkeletonSequences.ToList();

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
                this.sensor.ColorStream.Enable();
                this.sensor.ColorFrameReady += sensor_ColorFrameReady;

                this.sensor.SkeletonStream.Enable();
                this.sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;

                // Create the drawing group we'll use for drawing
                this.drawingGroup = new DrawingGroup();

                // Create an image source that we can use in our image control
                this.imageSource = new DrawingImage(this.drawingGroup);

                // Allocate space to put the pixels we'll receive
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

                // Display the drawing using our image control
                this.SkeletonImage.Source = this.imageSource;

                try
                {
                    Console.WriteLine("Starting the sensor");
                    this.sensor.Start();
                    Console.WriteLine("Sensor started");

                    Timer timer = new Timer(3000);
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Sensor start failed. " + ex.Message);
                    this.sensor = null;
                }
            }
            else
            {
                MessageBox.Show("No Kinect detected. Please connect the Kinect and restart the program");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.dataContext.SubmitChanges();

            if (this.sensor != null)
            {
                this.sensor.Stop();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.activeSequences.Count > 5 || this.savedSequences.Count == 0)
            {
                Console.WriteLine("Not enough saved/active sequences.");
                return;
            }

            GhostSkeletonSequence sequence;
            lock (lockObj)
            {
                int randomIndex = this.random.Next() % this.savedSequences.Count;
                sequence = this.savedSequences[randomIndex];
                this.activeSequences.Add(sequence);
            }
        }

        private void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    frameCount++;

                    using (DrawingContext dc = this.drawingGroup.Open())
                    {
                        // Copy the pixel data from the image to a temporary array
                        colorFrame.CopyPixelDataTo(this.colorPixels);

                        for (int i = 0; i < this.colorPixels.Length; i += 4)
                        {
                            // Convert image to grayscale
                            int avg = (this.colorPixels[i] + this.colorPixels[i + 1] + this.colorPixels[i + 2]) / 3;
                            byte avgByte = Convert.ToByte(avg);

                            this.colorPixels[i] = avgByte;
                            this.colorPixels[i + 1] = avgByte;
                            this.colorPixels[i + 2] = avgByte;
                            this.colorPixels[i + 3] = 255; // alpha channel
                            //this.colorPixels[i + 3] = Convert.ToByte(255 - avg);
                        }

                        // Write the pixel data into our bitmap
                        this.colorBitmap.WritePixels(
                            new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                            this.colorPixels,
                            this.colorBitmap.PixelWidth * sizeof(int),
                            0);
                        dc.DrawImage(this.colorBitmap, new Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));

                        lock (lockObj)
                        {
                            List<GhostSkeletonSequence> toRemove = new List<GhostSkeletonSequence>();
                            foreach (GhostSkeletonSequence sequence in activeSequences)
                            {
                                if (sequence.CurrentFrame >= sequence.SavedSkeletons.Count - 1)
                                {
                                    sequence.CurrentFrame = 0;
                                    toRemove.Add(sequence);
                                    continue;
                                }

                                GhostSkeleton skeleton = sequence.SavedSkeletons[sequence.CurrentFrame];
                                this.DrawBonesAndJoints(skeleton, dc);

                                sequence.CurrentFrame++;
                            }

                            toRemove.ForEach(sequence => activeSequences.Remove(sequence));
                        }
                    }
                }
            }
        }

        private void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
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
                            this.trackingIDsToSequences.Add(skel.TrackingId, sequence);
                        }
                        else
                        {
                            sequence = this.trackingIDsToSequences[skel.TrackingId];
                        }

                        unseenTrackingIDs.Remove(skel.TrackingId);

                        sequence.AddSkeleton(skel);
                    }
                }
            }

            if (!isSaving)
            {
                foreach (int trackingID in unseenTrackingIDs)
                {
                    if (!this.trackingIDsToSequences.ContainsKey(trackingID))
                    {
                        return;
                    }

                    Console.WriteLine("we have " + unseenTrackingIDs.Count + " unseen tracking IDs");

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (obj, args) =>
                    {
                        GhostSkeletonSequence sequence = this.trackingIDsToSequences[trackingID];
                        this.trackingIDsToSequences.Remove(trackingID);

                        sequence.FinalizeRecording();
                        isSaving = true;
                        Console.WriteLine("beginning save");

                        this.dataContext.GhostSkeletonSequences.InsertOnSubmit(sequence);
                        Console.WriteLine("saved!");
                        sequence.UpdateCache(GhostSkeletonSequence.DefaultInterpolationFactor);

                        Console.WriteLine("Locking down. updating saved sequences");
                        lock (lockObj)
                        {
                            this.savedSequences.Add(sequence);
                        }

                        Console.WriteLine("Saved sequences updated. Unlocking.");

                        isSaving = false;
                    };

                    worker.RunWorkerAsync();
                }


                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }
        #endregion
    }
}
