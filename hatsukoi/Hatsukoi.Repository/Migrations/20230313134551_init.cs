using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hatsukoi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "管理員Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "管理員的使用者ID"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "管理員名稱")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "產品大分類的Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "產品大分類的名稱"),
                    EditLastTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "資料最後更改時間"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "最後更改資料的")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "商品標籤Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "標籤名稱"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "最後更改資料的"),
                    EditLastTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "資料最後更改時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "商店Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplyName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店的負責人姓名"),
                    ApplyPhone = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店的負責人電話"),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店名稱"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店負責人的Email"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店實際所在位置(縣市)"),
                    ProductOrigin = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商品產地"),
                    SocialMedia = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店的社群媒體網址"),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "品牌介紹文字"),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false, comment: "商店創建日期"),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店照片logo網址"),
                    ModifiDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改商店資料時間"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "他的買家Id"),
                    Currency = table.Column<int>(type: "int", nullable: false, comment: "使用的幣別(1:台幣 2:美金)"),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店的負責人的身分證字號"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店登記地址"),
                    ApplicantEnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商店的負責人英文姓名"),
                    SecondEmail = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商店負責人的第二Email"),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店登記地址的郵遞區號"),
                    VacationFirstDay = table.Column<DateTime>(type: "date", nullable: true, comment: "在休假模式時，休假的第一天"),
                    VacationLastDay = table.Column<DateTime>(type: "date", nullable: true, comment: "在休假模式時，休假的最後一天"),
                    ShopBannerSquare = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家的設計館招牌(正方形)"),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商店的故事(介紹)"),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家收款帳號的銀行代碼"),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家收款帳號的分行代碼"),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家收款帳號的銀行帳號"),
                    BankAccountName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家收款帳號的戶名"),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "修改財務信箱與收款帳戶時，需輸入驗證碼證明身分。(在成為賣家時會給)"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "授權經銷品牌: hatsukoi"),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "統一編號(8碼數字)"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店照片icon網址"),
                    ShopBannerRect = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家的設計館招牌(長方形)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "使用者ID")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User姓名"),
                    Gender = table.Column<bool>(type: "bit", nullable: true, comment: "使用者的性別，0:男,1:女"),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false, comment: "使用者加入Hatsukoi日期"),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "使用者的頭貼"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "使用者的信箱(第三方登入的話也會有信箱)"),
                    IsEmailCertified = table.Column<bool>(type: "bit", nullable: false, comment: "信箱是否認證通過(第三方登入也會有信箱，會判定為已驗證)"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "帳號，(因為會有第三方登入，所以設為可為NULL)"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "密碼，(因為會有第三方登入，所以設為可為NULL)"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "第三方登入會有一組識別碼，有這個就不會有帳號密碼"),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "使用者電話"),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true, comment: "使用者的生日"),
                    MemberLevel = table.Column<int>(type: "int", nullable: false, comment: "會員等級，0:品藍,1:白銀,2:黃金,3:鑽石,4:尊爵"),
                    StoreStatus = table.Column<int>(type: "int", nullable: false, comment: "店家狀態,0:沒申請,1:審核中,2:駁回,3:通過,4:停權"),
                    ModifiDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改使用者資料時間"),
                    LastLoginTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "最後上線時間"),
                    IsEmailOrder = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 訂單狀態 通知設定"),
                    IsEmailHatsukoi = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 站內訊息 通知設定"),
                    IsEmailActivity = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 活動優惠 通知設定"),
                    IsEmailPersonal = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 個人化推薦 通知設定"),
                    IsEmailWeek = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 會員雙周報 通知設定"),
                    IsEmailMember = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 會員權益 通知設定"),
                    IsEmailFollow = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件 追蹤的商品和設計館 通知設定"),
                    IsEmailFocus = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件關注評價優惠券 通知設定")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "商品小分類Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "商品大分類的Id"),
                    SubCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "產品小分類的名稱"),
                    EditLastTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "資料最後更改時間"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "最後更改資料的")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategory_Category",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "優惠券的Id ( 由賣家發行的優惠券種類表 )")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<int>(type: "int", nullable: false, comment: "優惠券由哪個賣家發行"),
                    Discount = table.Column<decimal>(type: "decimal(5,2)", nullable: false, comment: "折扣數"),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "使用說明 ( 使用條件 )"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "優惠券建立時間"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "優惠券開始使用時間"),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "優惠券最後使用時間"),
                    PromoCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "自訂的優惠碼")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupon_Seller",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chatroom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "聊天訊息ID")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false, comment: "誰傳送訊息"),
                    ReceiverId = table.Column<int>(type: "int", nullable: false, comment: "誰收到訊息"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "訊息內容"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "新增訊息時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatroom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chatroom_User",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chatroom_User1",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "信用卡清單的Id"),
                    CreditNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, comment: "信用卡卡號"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "信用卡持有者的使用者Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credit_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavShop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "喜好商家的Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "user的ID"),
                    SellerId = table.Column<int>(type: "int", nullable: false, comment: "seller的Id"),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "加入清單時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavShop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavShop_Seller",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavShop_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "通知訊息的編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendTo = table.Column<int>(type: "int", nullable: false, comment: "誰接收訊息(來自UserId)"),
                    SendTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "通知發送時間"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "訊息內文(手風琴拉開後的顯示)"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "訊息標題(手風琴未拉開的顯示)"),
                    LinkTo = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "連結的網址(訊息內的button按鈕)"),
                    KindOfNotifi = table.Column<int>(type: "int", nullable: false, comment: "訊息的種類是哪一種(0:訂單通知)(1:帳號通知)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User1",
                        column: x => x.SendTo,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "訂單編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "訂單狀態( 1:尚未付款 2:處理中(買家已付款，賣家未出貨) 3:帶收貨 4:已完成 5:已取消(三天內沒付款，或是買家自行取消) 6:退貨申請中 7:已退貨)"),
                    RecipientAddress = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "收件地址"),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "收件人姓名"),
                    RecipientPhone = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "收件人電話"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false, comment: "訂單最終結帳金額"),
                    SellerId = table.Column<int>(type: "int", nullable: false, comment: "賣家Id ( 因為不能合併結帳，所以一張單只會有一個賣家 )"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "哪個帳號下訂單的"),
                    CouponId = table.Column<int>(type: "int", nullable: true, comment: "優惠券的Id，如果沒使用則為0"),
                    Evaluate = table.Column<int>(type: "int", nullable: true, comment: "訂單評價，(評分範圍可為1~5)"),
                    Payment = table.Column<int>(type: "int", nullable: true, comment: "付款方式 ( 1:轉帳、2:信用卡 )"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "訂單下單時間"),
                    PayTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "訂單付款完成時間"),
                    GreenPayId = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "綠界Id"),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "備註欄 當買家取消or退貨時 不為null"),
                    EvaluateText = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "訂單評價的留言文字"),
                    RecipientCity = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "收件人的城市"),
                    RecipientPostCode = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "收件人的郵遞區號"),
                    EvaluateDate = table.Column<DateTime>(type: "date", nullable: true, comment: "訂單評價的時間"),
                    StatusDealTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "訂單進入處理鐘的時間"),
                    StatusSendTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "賣家計出貨物的時間"),
                    StatusFinishTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "訂單完成時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Seller",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviewer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "user成為賣家的審核表 ID")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "誰申請的"),
                    ReviewTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "何時審核的"),
                    Reviewstatus = table.Column<int>(type: "int", nullable: false, comment: "審核狀態 1:未審核 2:審核失敗 3:審核成功"),
                    FailReason = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "審核失敗時，失敗的原因"),
                    ApplyName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "申請人姓名"),
                    ApplyPhone = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "申請人手機號碼"),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "品牌名稱 (設計館名稱)"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店負責人的Email"),
                    ProductOrigin = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商品產地"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店實際所在位置(縣市)"),
                    SocialMedia = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商店的社群媒體網址"),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "品牌介紹文字"),
                    AdministratorId = table.Column<int>(type: "int", nullable: true, comment: "審核者Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviewer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviewer_Administrator",
                        column: x => x.AdministratorId,
                        principalTable: "Administrator",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviewer_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "產品編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "產品名稱"),
                    SubCategory = table.Column<int>(type: "int", nullable: false, comment: "產品小分類Id(產品只會連到小分類，小分類才會連到大分類)"),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false, comment: "產品價格"),
                    ProductStatus = table.Column<int>(type: "int", nullable: false, comment: "1:正常供應 2:售罄 3:停產"),
                    SellerId = table.Column<int>(type: "int", nullable: false, comment: "誰賣這個商品"),
                    ViewTimes = table.Column<int>(type: "int", nullable: true, comment: "總瀏覽數"),
                    MadeCountry = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商品產地"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商品的描述"),
                    Editor = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商品介紹，用富文本編輯器轉HTML來儲存"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Seller",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_SubCategory",
                        column: x => x.SubCategory,
                        principalTable: "SubCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CouponList",
                columns: table => new
                {
                    CouponId = table.Column<int>(type: "int", nullable: false, comment: "票券的Id"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "票券的擁有者"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "票券狀態 ( 1:還不能使用 2:超過使用期限 3:可使用 4:已被使用 )"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "票券得到日期")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_CouponList_Coupon",
                        column: x => x.CouponId,
                        principalTable: "Coupon",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CouponList_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplyImg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "申請賣家要附五張照片，存這些照片的Id的表")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerId = table.Column<int>(type: "int", nullable: false, comment: "哪一次申請的Id"),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "照片的網址")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyImg_Reviewer",
                        column: x => x.ReviewerId,
                        principalTable: "Reviewer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "購物車Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "使用者Id"),
                    SellerId = table.Column<int>(type: "int", nullable: false, comment: "賣家Id"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "產品編號"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "產品數量")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cart_Seller",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cart_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "喜好商品的Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "user的ID"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "product的id"),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "加入清單時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavProduct_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavProduct_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "商品圖片Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "圖片本身(網址)"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "這個圖片屬於哪個商品"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "商品排序順位(同一個商品的照片排序，必須四張)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImg_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "商品規格的Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstSpecification = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商品第一種規格名稱(標題)"),
                    FirstSepcItem = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商品第一種規格的選項"),
                    SecondSpecification = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商品第二種規格名稱(標題)"),
                    SecondSepcItem = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "商品第二種規格的選項"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "商品的Id"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: true, comment: "這個規格如果有與Product價格不同時將會存在這個欄位")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecification_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductTagList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "商品標籤的清單")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTagId = table.Column<int>(type: "int", nullable: false, comment: "商品標籤Id"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "商品Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTagList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTagList_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTagList_ProductTag",
                        column: x => x.ProductTagId,
                        principalTable: "ProductTag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ViewTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "觀看次數記錄的Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "誰觀看這個商品"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "被觀看的商品Id"),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "瀏覽的時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewTime_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ViewTime_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "訂單明細編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "訂單標號"),
                    ProductSpecificationId = table.Column<int>(type: "int", nullable: false, comment: "商品規格Id ( 一個明細只能記錄一種商品，規格不同也算不同商品 )"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "商品個數數量"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false, comment: "商品單數原價(紀錄)"),
                    SellerName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "店家名稱(紀錄)"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "商品名稱(紀錄)"),
                    FirstSpecification = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "第一規格標題名稱(紀錄)"),
                    FirstSepcItem = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "第一規格選項名稱(紀錄)"),
                    SecondSpecification = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "第二規格標題名稱(紀錄)"),
                    SecondSepcItem = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "第二規格選項名稱(紀錄)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetail_ProductSpecification",
                        column: x => x.ProductSpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyImg_ReviewerId",
                table: "ApplyImg",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductId",
                table: "Cart",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_SellerId",
                table: "Cart",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chatroom_ReceiverId",
                table: "Chatroom",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chatroom_SenderId",
                table: "Chatroom",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_SellerId",
                table: "Coupon",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponList_CouponId",
                table: "CouponList",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponList_UserId",
                table: "CouponList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Credit_UserId",
                table: "Credit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavProduct_ProductId",
                table: "FavProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavProduct_UserId",
                table: "FavProduct",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavShop_SellerId",
                table: "FavShop",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavShop_UserId",
                table: "FavShop",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SendTo",
                table: "Notification",
                column: "SendTo");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SellerId",
                table: "Order",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductSpecificationId",
                table: "OrderDetail",
                column: "ProductSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SellerId",
                table: "Product",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SubCategory",
                table: "Product",
                column: "SubCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImg_ProductId",
                table: "ProductImg",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecification_ProductId",
                table: "ProductSpecification",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTagList_ProductId",
                table: "ProductTagList",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTagList_ProductTagId",
                table: "ProductTagList",
                column: "ProductTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviewer_AdministratorId",
                table: "Reviewer",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviewer_UserId",
                table: "Reviewer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_CategoryId",
                table: "SubCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewTime_ProductId",
                table: "ViewTime",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewTime_UserId",
                table: "ViewTime",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyImg");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Chatroom");

            migrationBuilder.DropTable(
                name: "CouponList");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "FavProduct");

            migrationBuilder.DropTable(
                name: "FavShop");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ProductImg");

            migrationBuilder.DropTable(
                name: "ProductTagList");

            migrationBuilder.DropTable(
                name: "ViewTime");

            migrationBuilder.DropTable(
                name: "Reviewer");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductSpecification");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Seller");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
