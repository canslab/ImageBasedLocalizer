using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageLocalizer
{
    public partial class Form1 : Form
    {
        List<String> selectedFolderPaths = new List<string>();
        GlobalDB globalDB = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = fbd.SelectedPath;
                selectedFolderList.Items.Add(folderPath);
                selectedFolderPaths.Add(folderPath);
            }
            constructDBButton.Enabled = true;
        }

        private void constructDBButton_Click(object sender, EventArgs e)
        {
            if (selectedFolderPaths.Count == 0)
            {
                MessageBox.Show("You should select valid folders!");
            }
            // Iterate over all files of each folder path in "selectedFolderPaths"
            foreach (var eachFolderPath in selectedFolderPaths)
            {
                Console.WriteLine(eachFolderPath);
            }
            Console.WriteLine("\n\n\n\n");

            globalDB = new GlobalDB(selectedFolderPaths, 150, 0.5f);

            foreach (var eachPlace in globalDB.Places)
            {
                trainedPlaceList.Items.Add(eachPlace.PlaceName);
            }

            Console.WriteLine("Finished training");
            constructDBButton.Enabled = false;
            localizeButton.Enabled = true;
            selectedFolderPaths.Clear();
            selectedFolderList.Items.Clear();
            evaluatePerformanceButton.Enabled = true;
        }

        private void localizeButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();

            float mostLieklyPlaceSimilarity = 0.0f;
            Tuple<int, int, int> mostLikelyPose = null;
            Place mostLikelyPlace = null;

            if (result == DialogResult.OK)
            {
                OpenCvSharp.Cv2.DestroyAllWindows();
                var fileNameList = ofd.FileNames;

                var queryImageMat = OpenCvSharp.Cv2.ImRead(fileNameList[0]);
                OpenCvSharp.Cv2.Resize(queryImageMat, queryImageMat, new OpenCvSharp.Size(queryImageMat.Width / 2, queryImageMat.Height / 2));
                OpenCvSharp.Cv2.ImShow("Query Image", queryImageMat);
                OpenCvSharp.Cv2.CvtColor(queryImageMat, queryImageMat, OpenCvSharp.ColorConversionCodes.BGR2GRAY);

                globalDB.GetNearestPlaceAndPose(queryImageMat, out mostLikelyPlace, out mostLikelyPose, out mostLieklyPlaceSimilarity);

                if (mostLikelyPose != null && mostLikelyPlace != null)
                {
                    Console.WriteLine("(x, y, theta) = ({0}, {1}, {2}), similarity=> {3}", mostLikelyPose.Item1, mostLikelyPose.Item2, mostLikelyPose.Item3, mostLieklyPlaceSimilarity);
                    var probableImagePath = mostLikelyPlace.RepImages[mostLikelyPose];

                    var probableImage = OpenCvSharp.Cv2.ImRead(probableImagePath);
                    OpenCvSharp.Cv2.Resize(probableImage, probableImage, new OpenCvSharp.Size(probableImage.Width / 2, probableImage.Height / 2));
                    OpenCvSharp.Cv2.ImShow("Most Likely Image", probableImage);
                }
                else
                {
                    Console.WriteLine("Cannot find the location... Localization failed..");
                }

            }
        }

        private void deleteClickedFolderButton_Click(object sender, EventArgs e)
        {
            if (selectedFolderList.SelectedIndex != -1)
            {
                selectedFolderList.Items.RemoveAt(selectedFolderList.SelectedIndex);
            }
            if (selectedFolderList.Items.Count == 0)
            {
                constructDBButton.Enabled = false;
            }
        }

        private void EvaluatePerformanceButton_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderPath = fbd.SelectedPath;

                var validImageFileList = Directory.GetFiles(folderPath);
                int nCorrect = 0;
                int nFeatureInsufficient = 0;
                int nIncorrect = 0;

                foreach (var eachValidationImagePath in validImageFileList)
                {
                    var tokens = eachValidationImagePath.Split('\\');
                    var imageFileName = tokens[tokens.Length - 1];
                    var parsedNameList = imageFileName.Split('_');

                    if (parsedNameList.Length != 5)
                    {
                        Console.WriteLine("Validation image name is invalid...");
                        continue;
                    }
                    else
                    {
                        var placeName = parsedNameList[0];
                        var groundTruth_str_x = parsedNameList[1];
                        var groundTruth_str_y = parsedNameList[2];
                        var groundTruth_str_thetaIdx = parsedNameList[3];

                        bool bParseX = int.TryParse(groundTruth_str_x, out int groundTruth_x);
                        bool bParseY = int.TryParse(groundTruth_str_y, out int groundTruth_y);
                        bool bParseThetaIdx = int.TryParse(groundTruth_str_thetaIdx, out int groundTruth_thetaIdx);

                        if (bParseX == true && bParseY == true && bParseThetaIdx == true)
                        {
                            globalDB.GetNearestPlaceAndPose(eachValidationImagePath, out var mostLikelyPlace, out var mostLikelyPose, out var mostLikelySimilarity);

                            if (mostLikelyPose.Item1 == groundTruth_x && mostLikelyPose.Item2 == groundTruth_y && mostLikelyPose.Item3 == groundTruth_thetaIdx && mostLikelySimilarity >= 0.2)
                            {
                                Console.WriteLine("Correct.. validation image name = {0}", imageFileName);
                                nCorrect++;
                            }
                            else
                            {
                                if (mostLikelySimilarity >= 0.2)
                                {
                                    Console.WriteLine("Incorrect.. validation image name = {0}, estimated result = {1}_{2}_{3}_{4}.png, similarity = {5}", imageFileName, mostLikelyPlace.PlaceName,
                                        mostLikelyPose.Item1, mostLikelyPose.Item2, mostLikelyPose.Item3, mostLikelySimilarity);
                                    nIncorrect++;
                                }
                                else
                                {
                                    Console.WriteLine("Cannot find the nearest one... lack of ORB features, validation image name = {0}", imageFileName);
                                    nFeatureInsufficient++;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Validation image name is invalid.. Try again");
                            continue;
                        }
                    }
                }
                Console.WriteLine("Correct # = {0} out of {1}", nCorrect, validImageFileList.Length);
                Console.WriteLine("Cannot find the nearest one due to lack of ORB features = {0}", nFeatureInsufficient);
                Console.WriteLine("Incorrect # = {0}", nIncorrect);
                MessageBox.Show(String.Format("Correct # => {0} out of {1}", nCorrect, validImageFileList.Length));
            }
            constructDBButton.Enabled = true;
        }

        private void LocalizeVideoButton_Click(object sender, EventArgs e)
        {
            var widthHeightInputDialog = new VideoLocalizationDialog();
            widthHeightInputDialog.ShowDialog();

            var mapWidth = widthHeightInputDialog.MapWidth;
            var mapHeight = widthHeightInputDialog.MapHeight;

            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();

            int pixelsPerGrid = 100;
            float[] directionX = { 10, 7.07f, 0, -7.07f, -10, -7.07f, 0, 7.07f };
            float[] directionY = { 0, -7.07f, -10, -7.07f, 0, 7.07f, 10, 7.07f };
            OpenCvSharp.Mat gridMap = new OpenCvSharp.Mat(mapHeight * pixelsPerGrid, mapWidth * pixelsPerGrid, OpenCvSharp.MatType.CV_8UC3, new OpenCvSharp.Scalar(255, 255, 255));

            // Preprocessing
            for (int rowIndex = 0; rowIndex < mapHeight; ++rowIndex)
            {
                var pt1 = new OpenCvSharp.Point(0, rowIndex * pixelsPerGrid);
                var pt2 = new OpenCvSharp.Point(pixelsPerGrid * mapWidth, rowIndex * pixelsPerGrid);

                gridMap.Line(pt1, pt2, new OpenCvSharp.Scalar(0, 0, 0));
            }
            for (int colIndex = 0; colIndex < mapWidth; ++colIndex)
            {
                var pt1 = new OpenCvSharp.Point(colIndex * pixelsPerGrid, 0);
                var pt2 = new OpenCvSharp.Point(colIndex * pixelsPerGrid, pixelsPerGrid * mapHeight);

                gridMap.Line(pt1, pt2, new OpenCvSharp.Scalar(0, 0, 0));
            }

            Dictionary<Place, int> placeVote = new Dictionary<Place, int>();

            if (result == DialogResult.OK)
            {
                var videoFileName = ofd.FileNames[0];
                if (videoFileName.EndsWith(".mov") || videoFileName.EndsWith("MOV"))
                {
                    var vc = OpenCvSharp.VideoCapture.FromFile(videoFileName);
                    Dictionary<Tuple<int, int, int>, int> poseRecords = new Dictionary<Tuple<int, int, int>, int>();
                    Tuple<int, int, int> mostLikelyPose = null;
                    Place mostLikelyPlace = null;
                    float mostLikelySimilarity = 0.0f;
                    int nIterations = 0;
                    Place localizedPlace = null;

                    while (vc.IsOpened())
                    {
                        OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
                        vc.Read(frame);

                        if (frame.Width == 0 || frame.Height == 0)
                        {
                            vc.Release();
                            break;
                        }

                        OpenCvSharp.Cv2.CvtColor(frame, frame, OpenCvSharp.ColorConversionCodes.BGR2GRAY);
                        OpenCvSharp.Cv2.Transpose(frame, frame);
                        OpenCvSharp.Cv2.Flip(frame, frame, OpenCvSharp.FlipMode.Y);

                        OpenCvSharp.Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 2, frame.Height / 2));
                        OpenCvSharp.Cv2.ImShow("Videos", frame);


                        if (nIterations < 15)
                        {
                            globalDB.GetNearestPlaceAndPose(frame, out mostLikelyPlace, out mostLikelyPose, out mostLikelySimilarity);
                            if (!placeVote.ContainsKey(mostLikelyPlace))
                            {
                                placeVote.Add(mostLikelyPlace, 0);
                            }
                            placeVote[mostLikelyPlace]++;
                        }
                        else if (nIterations == 15)
                        {
                            int currentMaxVote = 0;

                            foreach (var eachPlace in placeVote.Keys)
                            {
                                if (currentMaxVote <= placeVote[eachPlace])
                                {
                                    currentMaxVote = placeVote[eachPlace];
                                    localizedPlace = eachPlace;
                                }
                            }
                        }
                        else
                        {
                            globalDB.GetNearestPoseGivenPlaceName(frame, mostLikelyPlace.PlaceName, out mostLikelyPose, out mostLikelySimilarity);
                        }

                        if (mostLikelySimilarity < 0.20)
                        {
                            Console.WriteLine("Localization failed");
                        }
                        else
                        {
                            Console.WriteLine("(x, y, theta) = ({0}, {1}, {2}), similarity=> {3}", mostLikelyPose.Item1, mostLikelyPose.Item2, mostLikelyPose.Item3, mostLikelySimilarity);
                            if (poseRecords.ContainsKey(mostLikelyPose) == false)
                            {
                                poseRecords[mostLikelyPose] = 0;
                            }
                            poseRecords[mostLikelyPose]++;

                            if (poseRecords[mostLikelyPose] >= 5)
                            {
                                // For visualization code
                                //  => (pixelsPerGrid / 2) + pixelsPerGrid * (mostLikelyPose.Item1)
                                var arrowStartingPoint = new OpenCvSharp.Point((pixelsPerGrid / 2) + pixelsPerGrid * (mostLikelyPose.Item1), (pixelsPerGrid / 2) + pixelsPerGrid * (mostLikelyPose.Item2));
                                var arrowEndPoint = new OpenCvSharp.Point(arrowStartingPoint.X + directionX[mostLikelyPose.Item3 - 1] * 2, arrowStartingPoint.Y + directionY[mostLikelyPose.Item3 - 1] * 2);

                                OpenCvSharp.Cv2.ArrowedLine(gridMap, arrowStartingPoint, arrowEndPoint, new OpenCvSharp.Scalar(0, 0, 255), 2);
                                OpenCvSharp.Cv2.ImShow("Grid Map", gridMap);
                            }

                            nIterations++;
                        }
                        var key = OpenCvSharp.Cv2.WaitKey(1);
                        if ((char)key == 27) // ESC
                        {
                            break;
                        }


                    }
                }
                else
                {
                    MessageBox.Show("Your video file format is invalid");
                }
            }
            MessageBox.Show("Video Localization Done");
            OpenCvSharp.Cv2.DestroyAllWindows();
        }
    }
}
