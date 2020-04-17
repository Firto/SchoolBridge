namespace SchoolBridge.Helpers.DtoModels
{
    public class ResultDto
    {
        public bool ok { get; set; }
        public object result { get; set; }

        public static ResultDto Create(object obj)
            => new ResultDto { ok = true, result = obj };
    }
}