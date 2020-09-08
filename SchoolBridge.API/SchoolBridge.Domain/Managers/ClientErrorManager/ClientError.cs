using SchoolBridge.Helpers.DtoModels.ClientErrors.Info;

namespace SchoolBridge.Domain.Managers.CClientErrorManager
{
    public class ClientError
    {
        public string[] StringArguments { get; private set; }
        public string Description { get; private set; }
        public ClientError(string description, params string[] arguments)
        {
            Description = description;
            StringArguments = arguments;
        }

        public ClientErrorInfoDto GetInfo()
        {
            return new ClientErrorInfoDto
            {
                Description = Description
            };
        }
    }
}