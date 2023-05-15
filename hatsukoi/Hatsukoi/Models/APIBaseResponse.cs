namespace Hatsukoi.Models
{
    public class APIBaseResponse
    {
        public APIBaseResponse()
        {
            Status = APIStatus.Fail;
            ErrorMsg = "exception!";
        }
        /// <summary>
        /// 操作成功時回傳
        /// </summary>
        /// <param name="body">回傳的Body物件</param>
        public APIBaseResponse(object body)
        {
            Status = APIStatus.Success;
            ErrorMsg = "Success!";
            Result = body;
        }
        //檢查API狀態是否正確
        public APIStatus Status { get; set; }
        //如果錯誤的話的錯誤訊息
        public string ErrorMsg { get; set; }
        //成功的話傳到前端的物件
        public object Result { get; set; }

    }
    //
    public enum APIStatus
    {
        Success = 0,
        Fail = 1
    }
}
