using SchoolBridge.Helpers.DtoModels.ClientErrors.Info;

namespace SchoolBridge.Helpers.Managers.CClientErrorManager
{
    public class ClientError
    {
        public string Message { get; private set; }
        public ClientError(string Message) => this.Message = Message;

        public ClientErrorInfoDto GetInfo()
        {
            return new ClientErrorInfoDto
            {
                Message = Message
            };
        }
    }
}