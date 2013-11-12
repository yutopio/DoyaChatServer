using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using WFE.Models;

namespace WFE.Controllers
{
    public class ChatController : ApiController
    {
        static readonly CloudTable usersTable;

        static ChatController()
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(
                    RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
                var tableClient = storageAccount.CreateCloudTableClient();
                usersTable = tableClient.GetTableReference("users");
                usersTable.CreateIfNotExists();
            }
            catch
            {
                usersTable = null;
            }
        }

        [HttpPost, ActionName("Register")]
        public RegisterModel Register(RegisterModel reg)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(reg.GcmId);
            var hash = md5.ComputeHash(inputBytes);
            reg.Id = (((int)hash[0] << 24) | ((int)hash[1] << 16) | ((int)hash[2] << 8) | (int)hash[3]);
            var registerOp = TableOperation.InsertOrReplace(reg);
            var registerResult = usersTable.Execute(registerOp);
            return new RegisterModel { Id = reg.Id };
        }

        [HttpPost, ActionName("Send")]
        public void Send(int from, int to, [FromBody] string msg)
        {
            if (usersTable == null)
                throw new ApplicationException("usersTable == null");

            var registerOp = TableOperation.Retrieve<RegisterModel>("foo", to.ToString());
            var registerResult = usersTable.Execute(registerOp);
            var registerRow = registerResult.Result as RegisterModel;

            new GoogleCloudMessagingModel().Send<DoyaMessage<MessagePushModel>>(
                new GcmMessage<DoyaMessage<MessagePushModel>>
                {
                    RegistrationIds = new List<string> { registerRow.GcmId },
                    Data =
                        new DoyaMessage<MessagePushModel>
                        {
                            Tag = "msg",
                            Data = new MessagePushModel { SenderId = from, Message = msg }
                        }
                });
        }

        [HttpPost, ActionName("SendFace")]
        public void SendFace(int from, int to, [FromBody] string imgBase64)
        {
            if (usersTable == null)
                throw new ApplicationException("usersTable == null");

            var buf = Convert.FromBase64String(imgBase64);
            var result = new FaceDetectionModel().Send(buf);
            var face = result.FaceRecognition.DetectionFaceInfo;
            if (face == null) return;

            var registerOp = TableOperation.Retrieve<RegisterModel>("foo", to.ToString());
            var registerResult = usersTable.Execute(registerOp);
            var registerRow = registerResult.Result as RegisterModel;

            new GoogleCloudMessagingModel().Send<DoyaMessage<FacePushModel>>(
                new GcmMessage<DoyaMessage<FacePushModel>>
                {
                    RegistrationIds = new List<string> { registerRow.GcmId },
                    Data =
                        new DoyaMessage<FacePushModel>
                        {
                            Tag = "face",
                            Data = new FacePushModel
                            {
                                SenderId = from,
                                Gender = face.GenderJudge.genderResult,
                                LeftBrow = face.BlinkJudge.blinkLevel.leftEye > 50 ? 0 : 1,
                                LeftEye = face.BlinkJudge.blinkLevel.leftEye > 50 ? 0 : 1,
                                RightBrow = face.BlinkJudge.blinkLevel.rightEye > 50 ? 0 : 1,
                                RightEye = face.BlinkJudge.blinkLevel.rightEye > 50 ? 0 : 1,
                                Mouth = face.SmileJudge.smileLevel > 55 ? 0 : 1
                            }
                        }
                });
        }
    }
}
