using Microsoft.AspNetCore.Http;

namespace SchoolBridge.Helpers.Managers
{
    public static class UUIDHandler
    {
        public static bool CheckUUID(IHeaderDictionary headers, out string uuid)
        {
            if (headers.ContainsKey("UUID"))
                uuid = headers["UUID"].ToString();
            else uuid = null;

            return uuid != null && uuid.Length > 10 && uuid.Length < 100;
        }

        public static string GetUUID(IHeaderDictionary headers)
        {
            string uuid;
            CheckUUID(headers, out uuid);
            return uuid;
        }
    }
}
