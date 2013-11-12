using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using WFE.Models;

namespace WFE.Controllers
{
    public class ChatController : ApiController
    {
        static readonly CloudTable usersTable;

        static ChatController()
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(
                    RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

                Trace.TraceInformation("Creating table client.");
                var tableClient = storageAccount.CreateCloudTableClient();
                usersTable = tableClient.GetTableReference("users");
                usersTable.CreateIfNotExists();
            }
            catch { }
        }

        [HttpPost, ActionName("Register")]
        public RegisterModel Register(RegisterModel reg)
        {
            reg.Id = (int)DateTime.Now.Ticks;
            var registerOp = TableOperation.InsertOrReplace(reg);
            var registerResult = usersTable.Execute(registerOp);
            return new RegisterModel { Id = reg.Id };
        }

        [HttpPost, ActionName("Send")]
        public void Send(int from, int to, [FromBody] string msg)
        {
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
            Convert.FromBase64String(imgBase64);

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
                            Data = new FacePushModel { SenderId = from }
                        }
                });
        }
    }
}
