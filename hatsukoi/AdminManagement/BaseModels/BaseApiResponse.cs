using AdminManagement.Enums;

namespace AdminManagement.BaseModels
{
    public class BaseApiResponse
    {
        public BaseApiResponse()
        {

        }
        public BaseApiResponse(object body)
        {
            Body= body;
            IsSuccess=true;
            Code = ApiStatusEnum.Success;
        }
        public object Body { get; set; }
        public ApiStatusEnum Code { get; set; }
        public bool IsSuccess { get; set; }
    }
}
