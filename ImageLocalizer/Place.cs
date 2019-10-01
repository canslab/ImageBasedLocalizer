using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace ImageLocalizer
{
    class Place
    {
        public string PlaceName { get; private set; }
        public int Size { get; private set; }
        public Dictionary<Tuple<int, int, int>, List<Mat>> GridDescriptor { get; private set; }
        private ORB m_orbDetector = null;
        private BFMatcher m_matcher = new OpenCvSharp.BFMatcher(NormTypes.Hamming2);
        private int m_nORBFeatures = 0;
        public Dictionary<Tuple<int, int, int>, String> RepImages { get; private set; }

        public Place(string[] images, int nORBFeatures, float scaleFactor)
        {
            m_orbDetector = ORB.Create(nORBFeatures);
            GridDescriptor = new Dictionary<Tuple<int, int, int>, List<Mat>>();
            RepImages = new Dictionary<Tuple<int, int, int>, string>();
            m_nORBFeatures = nORBFeatures;

            // Iterate over all image paths in images
            foreach (var imagePath in images)
            {
                Tuple<int, int, int> currentImagePose = null;

                // Read the current Image
                var currentImageMat = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
                Cv2.Resize(currentImageMat, currentImageMat, new OpenCvSharp.Size(currentImageMat.Width * scaleFactor, currentImageMat.Height * scaleFactor));

                var temp = imagePath.Split('\\');
                var fileName = temp[temp.Length - 1];

                // Tokenize to extract the information from the image
                string[] tokens = fileName.Split('_');

                if (tokens.Length == 5)
                {
                    int x, y, rotIdx;
                    bool bParseSuccess = true;

                    bParseSuccess = Int32.TryParse(tokens[1], out x);
                    if (bParseSuccess == false)
                    {
                        // Parse Failed..
                        Console.WriteLine("Check names of images");
                    }
                    bParseSuccess = Int32.TryParse(tokens[2], out y);
                    if (bParseSuccess == false)
                    {
                        // Parse Failed..
                        Console.WriteLine("Check names of images");
                    }

                    bParseSuccess = Int32.TryParse(tokens[3], out rotIdx);
                    if (bParseSuccess == false)
                    {
                        // Parse Failed..
                        Console.WriteLine("Check names of images");
                    }

                    // Allocate the coordinate for this image
                    PlaceName = tokens[0];
                    currentImagePose = new Tuple<int, int, int>(x, y, rotIdx);

                    // Increment the count of images for this place
                    Size++;

                    // If there is no key (currentImageLocation), allocate
                    if (GridDescriptor.ContainsKey(currentImagePose) == false)
                    {
                        GridDescriptor.Add(currentImagePose, new List<Mat>(0));
                        RepImages.Add(currentImagePose, imagePath);
                    }

                    // Detect and compute using ORB
                    Mat descriptor = new Mat();
                    KeyPoint[] keypoints;

                    m_orbDetector.DetectAndCompute(currentImageMat, null, out keypoints, descriptor);
                    GridDescriptor[currentImagePose].Add(descriptor);
                    Console.WriteLine("Storing Images.. Place = {0}, (x={1}, y={2}, thetaIdx={3})", PlaceName, currentImagePose.Item1, currentImagePose.Item2, currentImagePose.Item3);
                }
            }

        }

        private void CalculateSimilarity(Mat query_desc, Mat train_desc, out List<float> distances)
        {
            distances = new List<float>();
            if (train_desc.Rows >= 2)
            {
                var matches = m_matcher.KnnMatch(query_desc, train_desc, 2);

                for (int i = 0; i < matches.Length; ++i)
                {
                    if (matches[i][0].Distance < 0.75 * matches[i][1].Distance)
                    {
                        distances.Add(matches[i][0].Distance);
                    }
                }
            }
        }

        //public void GetNearestOne(string givenImageFullPath, out Tuple<int, int, int> mostSimilarPose, out float maxSimilarity)
        //{
        //    KeyPoint[] keypoints = null;
        //    Mat queryDescriptor = new Mat();

        //    var queryMat = Cv2.ImRead(givenImageFullPath, OpenCvSharp.ImreadModes.Grayscale);
        //    Cv2.Resize(queryMat, queryMat, new OpenCvSharp.Size(queryMat.Width / 2, queryMat.Height / 2));
        //    m_orbDetector.DetectAndCompute(queryMat, null, out keypoints, queryDescriptor);

        //    GetNearestOne(queryMat, out mostSimilarPose, out maxSimilarity);
        //}
          
        public void GetNearestOne(Mat givenImageMat, out Tuple<int, int, int> mostSimilarPose, out float maxSimilarity)
        {
            KeyPoint[] keypoints = null;
            Mat queryDescriptor = new Mat();
            m_orbDetector.DetectAndCompute(givenImageMat, null, out keypoints, queryDescriptor);

            Tuple<int, int, int> currentMaxPose = null;
            float currentMaxVal = 0.0f;

            // For each pose, find the nearest image! 
            foreach (var eachPose in this.GridDescriptor.Keys)
            {
                List<float> similaritiesForEachPose = new List<float>();
                var correspondingDescriptorList = GridDescriptor[eachPose];

                foreach (var eachDescriptor in correspondingDescriptorList)
                {
                    CalculateSimilarity(queryDescriptor, eachDescriptor, out List<float> distances);
                    similaritiesForEachPose.Add(distances.Count);
                }

                if (currentMaxVal <= similaritiesForEachPose.Max())
                {
                    currentMaxPose = eachPose;
                    currentMaxVal = similaritiesForEachPose.Max();
                }
            }

            mostSimilarPose = currentMaxPose;
            maxSimilarity = currentMaxVal / m_nORBFeatures;
        }
    }
}
