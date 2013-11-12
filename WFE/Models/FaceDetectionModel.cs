using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Xml.Serialization;

namespace WFE.Models
{
    public class FaceDetectionModel : JsonApiProxy
    {
        public override string BaseUri
        {
            get { return ConfigurationManager.AppSettings["PuxApiEndpoint"]; }
        }

        public DetectionResult Send(byte[] buf)
        {
            var client = NewClient();
            var content = new MultipartFormDataContent("====");
            content.Add(new StringContent(ConfigurationManager.AppSettings["PuxApiKey"]), "apiKey");
            var bcont = new ByteArrayContent(buf);
            bcont.Headers.Add("Content-Type", "image/jpeg");
            content.Add(bcont, "inputFile", "detect.jpg");
            var response = client.PostAsync("face.do", content).Result;

            var serializer = new XmlSerializer(typeof(DetectionResult));
            var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result);
            var ret = (DetectionResult)serializer.Deserialize(reader);
            reader.Close();

            return ret;
        }
    }

    [Serializable]
    [XmlRoot("results")]
    public class DetectionResult
    {
        [XmlAttribute("version")]
        public string Version;
        [XmlElement("faceRecognition")]
        public FaceRecognition FaceRecognition;
    }

    public class FaceRecognition
    {
        [XmlElement("width")]
        public int Width;
        [XmlElement("height")]
        public int Height;
        [XmlElement("detectionFaceNumber")]
        public int DetectionFaceNumber;
        [XmlElement("detectionFaceInfo")]
        public DetectionFaceInfo DetectionFaceInfo;
    }

    public class DetectionFaceInfo
    {
        [XmlElement("faceCoordinates")]
        public FaceCoordinates FaceCoordinates;
        [XmlElement("facePartsCoordinates")]
        public FacePartsCoordinates FacePartsCoordinates;
        [XmlElement("blinkJudge")]
        public BlinkJudge BlinkJudge;
        [XmlElement("ageJudge")]
        public AgeJudge AgeJudge;
        [XmlElement("genderJudge")]
        public GenderJudge GenderJudge;
        [XmlElement("faceAngleJudge")]
        public FaceAngleJudge FaceAngleJudge;
        [XmlElement("smileJudge")]
        public SmileJudge SmileJudge;
    }

    public class FaceCoordinates
    {
        public int faceConfidence;
        public int faceFrameLeftX;
        public int faceFrameRightX;
        public int faceFrameTopY;
        public int faceFrameBottomY;
        public int leftEyeX;
        public int leftEyeY;
        public int rightEyeX;
        public int rightEyeY;
    }

    public class FacePartsCoordinates
    {
        public Position boundingRectangleLeftUpper;
        public Position boundingRectangleRightUpper;
        public Position boundingRectangleRightLower;
        public Position boundingRectangleLeftLower;
        public Position leftBlackEyeCenter;
        public Position rightBlackEyeCenter;
        public Position basicCoordinateNumber;
        public Position leftCheek1;
        public Position leftCheek2;
        public Position leftCheek3;
        public Position leftCheek4;
        public Position leftCheek5;
        public Position leftCheek6;
        public Position leftCheek7;
        public Position chin;
        public Position rightCheek7;
        public Position rightCheek6;
        public Position rightCheek5;
        public Position rightCheek4;
        public Position rightCheek3;
        public Position rightCheek2;
        public Position rightCheek1;
        public Position parietal;
        public Position rightEyebrowOutsideEnd;
        public Position rightEyebrowUpperOutside;
        public Position rightEyebrowUpperInside;
        public Position rightEyebrowInsideEnd;
        public Position rightEyebrowLowerInside;
        public Position rightEyebrowLowerOutside;
        public Position leftEyebrowOutsideEnd;
        public Position leftEyebrowUpperOutside;
        public Position leftEyebrowUpperInside;
        public Position leftEyebrowInsideEnd;
        public Position leftEyebrowLowerInside;
        public Position leftEyebrowLowerOutside;
        public Position leftEyeOutsideEnd;
        public Position leftEyeUpperOutside;
        public Position leftEyeUpperCenter;
        public Position leftEyeUpperInside;
        public Position leftEyeInsideEnd;
        public Position leftEyeLowerInside;
        public Position leftEyeLowerCenter;
        public Position leftEyeLowerOutside;
        public Position rightEyeOutsideEnd;
        public Position rightEyeUpperOutside;
        public Position rightEyeUpperCenter;
        public Position rightEyeUpperInside;
        public Position rightEyeInsideEnd;
        public Position rightEyeLowerInside;
        public Position rightEyeLowerCenter;
        public Position rightEyeLowerOutside;
        public Position noseLeftLineUpper;
        public Position noseLeftLineCenter;
        public Position noseLeftLineLower0;
        public Position noseLeftLineLower1;
        public Position noseBottomCenter;
        public Position noseRightLineLower1;
        public Position noseRightLineLower0;
        public Position noseRightLineCenter;
        public Position noseRightLineUpper;
        public Position nostrilsLeft;
        public Position nostrilsRight;
        public Position noseTip;
        public Position noseCenterLine0;
        public Position noseCenterLine1;
        public Position mouthLeftEnd;
        public Position upperLipUpperLeftOutside;
        public Position upperLipUpperLeftInside;
        public Position mouthUpperPart;
        public Position upperLipUpperRightInside;
        public Position upperLipUpperRightOutside;
        public Position mouthRightEnd;
        public Position lowerLipLowerRightOutside;
        public Position lowerLipLowerRightInside;
        public Position mouthLowerPart;
        public Position lowerLipLowerLeftInside;
        public Position lowerLipLowerLeftOutside;
        public Position upperLipLowerLeft;
        public Position upperLipLowerCenter;
        public Position upperLipLowerRight;
        public Position lowerLipUpperRight;
        public Position lowerLipUpperCenter;
        public Position lowerLipUpperLeft;
        public Position mouthCenter;
        public int detectionPointTotal;
    }

    public class BlinkJudge
    {
        public BlinkLevel blinkLevel;
    }

    public class BlinkLevel
    {
        [XmlAttribute("leftEye")]
        public int leftEye;
        [XmlAttribute("rightEye")]
        public int rightEye;
    }

    public class AgeJudge
    {
        public int ageResult;
        public int ageScore;
    }
    
    public class GenderJudge
    {
        public int genderResult;
        public int genderScore;
    }
    
    public class FaceAngleJudge
    {
        public int pitch;
        public int yaw;
        public int roll;
        public Position faceAngleVector;
        public Position gazeVector;
    }

    public class SmileJudge
    {
        public int smileLevel;
    }

    public class Position
    {
        [XmlAttribute("x")]
        public int X;
        [XmlAttribute("y")]
        public int Y;
    }
}
