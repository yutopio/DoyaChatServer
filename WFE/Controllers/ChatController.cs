using System;
using System.Web.Http;
using WFE.Models;

namespace WFE.Controllers
{
    public class ChatController : ApiController
    {
        [HttpPost, ActionName("Register")]
        public RegisterResponseModel Register(RegisterRequestModel reg)
        {
            return null;
        }

        [HttpPost, ActionName("Send")]
        public void Send(MessageModel msg)
        {
        }

        [HttpPost, ActionName("SendFace")]
        public void SendFace(int from, int to)
        {
        }
    }
}
