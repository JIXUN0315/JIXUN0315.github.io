using System.ComponentModel;

namespace Hatsukoi.Common
{
    public class HatsukoiEnum
    {
        /// <summary>
        /// 店家狀態,0:沒申請,1:審核中,2:駁回,3:通過,4:停權
        /// </summary>
        public enum StoreStatus
        {
            /// <summary>
            /// 沒申請
            /// </summary>
            NotApply = 0,
            /// <summary>
            /// 審核中
            /// </summary>
            UnderReview = 1,
            /// <summary>
            /// 駁回
            /// </summary>
            Reject = 2,
            /// <summary>
            /// 通過
            /// </summary>
            Pass = 3,
            /// <summary>
            /// 停權
            /// </summary>
            Disable = 4
        }
        public enum MemberLevel
        {
            /// <summary>
            /// 品藍會員
            /// </summary>
            Blue =0,
            /// <summary>
            /// 白銀會員
            /// </summary>
            Silver = 1,
            /// <summary>
            /// 黃金會員
            /// </summary>
            Gold = 2,
            /// <summary>
            /// 鑽石會員
            /// </summary>
            Diamond = 3,
            /// <summary>
            /// 尊爵會員
            /// </summary>
            Monarch = 4
        }
        public enum OrderStatus
        {
            NotFound = 0,
            /// <summary>
            /// 尚未付款
            /// </summary>
            NotPay = 1,
            /// <summary>
            /// 處理中(買家已付款，賣家未出貨)
            /// </summary>
            NotShipped = 2,
            /// <summary>
            /// 待收貨
            /// </summary>
            ToBeReceived = 3,
            /// <summary>
            /// 已完成
            /// </summary>
            Completed = 4,
            /// <summary>
            /// 已取消(三天內沒付款，或是買家自行取消)
            /// </summary>
            Cancelled = 5,
            /// <summary>
            /// 退貨申請中
            /// </summary>
            ReturnRequest = 6,
            /// <summary>
            /// 已退貨
            /// </summary>
            Returned = 7
        }
        public enum EmailType
        {
            CheckEmail = 0,
            ResetPassword = 1,
            ApplySeller = 2,
            BeSellerSuccess = 3,
            BindEmail = 4,
            ChangeEmail = 5,
            EditSellerSecondEmail = 6
        }
        public enum ExternalLoginType
        {
            Google = 0,
            Facebook = 1,
            Line = 2,
            Hatsukoi = 3
        }
        public enum Reviewstatus
        {
            NotReview = 0,
            Fail = 1,
            Success = 2
        }
        public enum WhichYear
        {
            ThisYear = 0,
            NextYear = 1
        }
        public enum SellerInfo
        {
            forProduct = 0,
            forShop = 1,
        }
        public enum ReviewInfo
        {
            forProduct = 0,
            forShop = 1,
        }

        public enum ProductStatus
        {
            //資料庫: ProductStatus 1:未上架 2:已上架
            //Enum:  已下架 offShelve = 1, 已上架 shelve = 2
            offShelve = 1,
            shelve = 2,
            delete = 3 //只能在資料庫看到, 後台畫面看不到
        }
        public enum Rating
        {
            all=0,
            onePoint=1, 
            twoPoint=2,
            threePoint=3,
            fourPoint=4,
            fivePoint=5
        }
        public enum CouponStatus
        {
            //還不能使用 
            invalid=0,
            //超過使用期限
            Expired=1,
            //可使用
            usable=2,
            //已被使用
            Used=3
        }
        public enum MessageType
        {
            UserSend=0,
            SellerSend=1
        }
        public enum DeleteStatus
        {
            None=0,
            Delete=1
        }
    }
}
