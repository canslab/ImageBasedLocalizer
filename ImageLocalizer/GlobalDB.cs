using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLocalizer
{
    class GlobalDB
    {
        public int NumORBFeatures { get; private set; }
        public List<Place> Places { get; private set; }
        public float ScaleFactor { get; private set; }

        public GlobalDB(List<String> folderPaths, int nORBFeatures, float scaleFactor)
        {
            Places = new List<Place>();
            NumORBFeatures = nORBFeatures;
            ScaleFactor = scaleFactor;
            foreach (var eachFolderPath in folderPaths)
            {
                var files = Directory.GetFiles(eachFolderPath, "*.png");
                var eachPlace = new Place(files, nORBFeatures, ScaleFactor);

                Places.Add(eachPlace);
            }
        }

        public void AddPlace(string[] imageFiles)
        {
            Place newPlace = new Place(imageFiles, NumORBFeatures, ScaleFactor);
            Places.Add(newPlace);
        }

        public void GetNearestPlaceAndPose(OpenCvSharp.Mat queryImageMat, out Place mostLikelyPlace, out Tuple<int,int,int> mostLikelyPose, out float mostLikelySimilarity)
        {
            float currentMaxSimilarity = 0.0f;
            Place currentMaxPlace = null;
            Tuple<int, int, int> currentMostLikelyPose = null;

            foreach (var eachPlace in Places)
            {
                eachPlace.GetNearestOne(queryImageMat, out Tuple<int, int, int> likelyPose, out float likelyPoseSimilarity);
                if (currentMaxSimilarity <= likelyPoseSimilarity)
                {
                    currentMaxSimilarity = likelyPoseSimilarity;
                    currentMaxPlace = eachPlace;
                    currentMostLikelyPose = likelyPose;
                }
            }

            mostLikelyPlace = currentMaxPlace;
            mostLikelySimilarity = currentMaxSimilarity;
            mostLikelyPose = currentMostLikelyPose;
        }

        public void GetNearestPlaceAndPose(string queryImagePath, out Place mostLikelyPlace, out Tuple<int, int, int> mostLikelyPose, out float mostLikelySimilarity)
        {
            var queryImageMat = OpenCvSharp.Cv2.ImRead(queryImagePath, OpenCvSharp.ImreadModes.Grayscale);
            OpenCvSharp.Cv2.Resize(queryImageMat, queryImageMat, new OpenCvSharp.Size(queryImageMat.Width / 2, queryImageMat.Height / 2));
            GetNearestPlaceAndPose(queryImageMat, out mostLikelyPlace, out mostLikelyPose, out mostLikelySimilarity);
        }

        public bool GetNearestPoseGivenPlaceName(OpenCvSharp.Mat queryImageMat , string placeName, out Tuple<int, int, int> mostLikelyPose, out float similarity)
        {
            int foundIndex = -1;

            for (int i = 0; i < Places.Count; ++i)
            {
                if (Places[i].PlaceName.Equals(placeName))
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex == -1)
            {
                mostLikelyPose = null;
                similarity = 0.0f;
                return false;
            }
            else
            {

                Places[foundIndex].GetNearestOne(queryImageMat, out mostLikelyPose, out similarity);
                return true;
            }            
        }
    }
}
