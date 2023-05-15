using System;
using System.Collections.Generic;
using Hatsukoi.Repository.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace Hatsukoi.Repository.DataContext;

public partial class HatsukoiContext : DbContext
{
    public HatsukoiContext()
    {
    }

    public HatsukoiContext(DbContextOptions<HatsukoiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<ApplyImg> ApplyImgs { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CenterNotify> CenterNotifies { get; set; }

    public virtual DbSet<ChatGpt> ChatGpts { get; set; }

    public virtual DbSet<ChatOnline> ChatOnlines { get; set; }

    public virtual DbSet<Chatroom> Chatrooms { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponList> CouponLists { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<FavProduct> FavProducts { get; set; }

    public virtual DbSet<FavShop> FavShops { get; set; }

    public virtual DbSet<LineMessage> LineMessages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImg> ProductImgs { get; set; }

    public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }

    public virtual DbSet<ProductTag> ProductTags { get; set; }

    public virtual DbSet<ProductTagList> ProductTagLists { get; set; }

    public virtual DbSet<Reviewer> Reviewers { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<SellerChatOnline> SellerChatOnlines { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ViewTime> ViewTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:HatsukoiContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasComment("管理員Id");
            entity.Property(e => e.Name).HasComment("管理員名稱");
            entity.Property(e => e.UserId).HasComment("管理員的使用者ID");
        });

        modelBuilder.Entity<ApplyImg>(entity =>
        {
            entity.ToTable("ApplyImg");

            entity.HasIndex(e => e.ReviewerId, "IX_ApplyImg_ReviewerId");

            entity.Property(e => e.Id).HasComment("申請賣家要附五張照片，存這些照片的Id的表");
            entity.Property(e => e.ImgUrl).HasComment("照片的網址");
            entity.Property(e => e.ReviewerId).HasComment("哪一次申請的Id");

            entity.HasOne(d => d.Reviewer).WithMany(p => p.ApplyImgs)
                .HasForeignKey(d => d.ReviewerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplyImg_Reviewer");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.ToTable("Banner");

            entity.Property(e => e.Id).HasComment("banner圖片Id");
            entity.Property(e => e.ImgUrl).HasComment("圖片網址");
            entity.Property(e => e.Sort).HasComment("圖片順序");
            entity.Property(e => e.Status).HasComment("圖片狀態(1:顯示,0:不顯示)");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.HasIndex(e => e.ProductId, "IX_Cart_ProductId");

            entity.HasIndex(e => e.SellerId, "IX_Cart_SellerId");

            entity.HasIndex(e => e.UserId, "IX_Cart_UserId");

            entity.Property(e => e.Id).HasComment("購物車Id");
            entity.Property(e => e.ProductId).HasComment("產品編號");
            entity.Property(e => e.Quantity).HasComment("產品數量");
            entity.Property(e => e.SellerId).HasComment("賣家Id");
            entity.Property(e => e.UserId).HasComment("使用者Id");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Product");

            entity.HasOne(d => d.Seller).WithMany(p => p.Carts)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Seller");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).HasComment("產品大分類的Id");
            entity.Property(e => e.CategoryName).HasComment("產品大分類的名稱");
            entity.Property(e => e.EditLastTime)
                .HasComment("資料最後更改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasComment("最後更改資料的");
        });

        modelBuilder.Entity<CenterNotify>(entity =>
        {
            entity.ToTable("CenterNotify");

            entity.Property(e => e.Id).HasComment("管理員後台發出通知給所有買家的Id");
            entity.Property(e => e.AdminId).HasComment("哪個管理員發出的");
            entity.Property(e => e.LinkTo).HasComment("連結的網址(訊息內的button按鈕)");
            entity.Property(e => e.SendTime)
                .HasComment("通知發送時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Text).HasComment("訊息內文(手風琴拉開後的顯示)");
            entity.Property(e => e.Title).HasComment("訊息標題(手風琴未拉開的顯示)");
        });

        modelBuilder.Entity<ChatGpt>(entity =>
        {
            entity.ToTable("ChatGPT");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MessageType).HasComment("0:GTP對User 1:GTP對Seller");
        });

        modelBuilder.Entity<ChatOnline>(entity =>
        {
            entity.ToTable("ChatOnline");

            entity.Property(e => e.Id).HasComment("聊天室線上人員資料表的Id");
            entity.Property(e => e.ConnectionId).HasComment("當連線進聊天室時會獲得一組連線ID");
            entity.Property(e => e.UserId).HasComment("聊天時的User身分");

            entity.HasOne(d => d.User).WithMany(p => p.ChatOnlines)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatOnline_User");
        });

        modelBuilder.Entity<Chatroom>(entity =>
        {
            entity.ToTable("Chatroom");

            entity.HasIndex(e => e.SellerId, "IX_Chatroom_ReceiverId");

            entity.HasIndex(e => e.UserId, "IX_Chatroom_SenderId");

            entity.Property(e => e.Id).HasComment("聊天訊息ID");
            entity.Property(e => e.CreateTime)
                .HasComment("新增訊息時間")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasComment("訊息為 1.已讀 或 2.未讀");
            entity.Property(e => e.Message).HasComment("訊息內容");
            entity.Property(e => e.MessageType).HasComment("1為User發送的，2為Seller發送的");
            entity.Property(e => e.SellerId).HasComment("誰收到訊息");
            entity.Property(e => e.UserId).HasComment("誰傳送訊息");

            entity.HasOne(d => d.Seller).WithMany(p => p.Chatrooms)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chatroom_Seller");

            entity.HasOne(d => d.User).WithMany(p => p.Chatrooms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chatroom_User2");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.ToTable("Coupon");

            entity.HasIndex(e => e.SellerId, "IX_Coupon_SellerId");

            entity.Property(e => e.Id).HasComment("優惠券的Id ( 由賣家發行的優惠券種類表 )");
            entity.Property(e => e.Condition)
                .HasComment("使用說明 ( 使用條件 )")
                .HasColumnType("decimal(8, 2)");
            entity.Property(e => e.CreateTime)
                .HasComment("優惠券建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.DeleteStatus).HasComment("優惠券是否被刪除了(0:未刪除 1:已刪除)");
            entity.Property(e => e.Discount)
                .HasComment("折扣數")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EndTime)
                .HasComment("優惠券最後使用時間")
                .HasColumnType("datetime");
            entity.Property(e => e.PromoCode).HasComment("自訂的優惠碼");
            entity.Property(e => e.SellerId).HasComment("優惠券由哪個賣家發行");
            entity.Property(e => e.StartTime)
                .HasComment("優惠券開始使用時間")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Seller).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Coupon_Seller");
        });

        modelBuilder.Entity<CouponList>(entity =>
        {
            entity.ToTable("CouponList");

            entity.HasIndex(e => e.CouponId, "IX_CouponList_CouponId");

            entity.HasIndex(e => e.UserId, "IX_CouponList_UserId");

            entity.Property(e => e.Id).HasComment("誰擁有優惠券的清單的Id");
            entity.Property(e => e.CouponId).HasComment("票券的Id");
            entity.Property(e => e.CreateTime)
                .HasComment("票券得到日期")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasComment("票券狀態 ( 1:還不能使用 2:超過使用期限 3:可使用 4:已被使用 )");
            entity.Property(e => e.UserId).HasComment("票券的擁有者");

            entity.HasOne(d => d.Coupon).WithMany(p => p.CouponLists)
                .HasForeignKey(d => d.CouponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponList_Coupon");

            entity.HasOne(d => d.User).WithMany(p => p.CouponLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponList_User");
        });

        modelBuilder.Entity<Credit>(entity =>
        {
            entity.ToTable("Credit");

            entity.HasIndex(e => e.UserId, "IX_Credit_UserId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("信用卡清單的Id");
            entity.Property(e => e.CreditNumber)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasComment("信用卡卡號");
            entity.Property(e => e.UserId).HasComment("信用卡持有者的使用者Id");

            entity.HasOne(d => d.User).WithMany(p => p.Credits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Credit_User");
        });

        modelBuilder.Entity<FavProduct>(entity =>
        {
            entity.ToTable("FavProduct");

            entity.HasIndex(e => e.ProductId, "IX_FavProduct_ProductId");

            entity.HasIndex(e => e.UserId, "IX_FavProduct_UserId");

            entity.Property(e => e.Id).HasComment("喜好商品的Id");
            entity.Property(e => e.ProductId).HasComment("product的id");
            entity.Property(e => e.Time)
                .HasComment("加入清單時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasComment("user的ID");

            entity.HasOne(d => d.Product).WithMany(p => p.FavProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavProduct_Product");

            entity.HasOne(d => d.User).WithMany(p => p.FavProducts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavProduct_User");
        });

        modelBuilder.Entity<FavShop>(entity =>
        {
            entity.ToTable("FavShop");

            entity.HasIndex(e => e.SellerId, "IX_FavShop_SellerId");

            entity.HasIndex(e => e.UserId, "IX_FavShop_UserId");

            entity.Property(e => e.Id).HasComment("喜好商家的Id");
            entity.Property(e => e.SellerId).HasComment("seller的Id");
            entity.Property(e => e.Time)
                .HasComment("加入清單時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasComment("user的ID");

            entity.HasOne(d => d.Seller).WithMany(p => p.FavShops)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavShop_Seller");

            entity.HasOne(d => d.User).WithMany(p => p.FavShops)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavShop_User");
        });

        modelBuilder.Entity<LineMessage>(entity =>
        {
            entity.ToTable("LineMessage");

            entity.Property(e => e.Id).HasComment("管理員後台發出Line給所有買家的Id");
            entity.Property(e => e.AdminId).HasComment("哪個管理員發出的");
            entity.Property(e => e.SendTime)
                .HasComment("通知發送時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Text).HasComment("訊息內文(手風琴拉開後的顯示)");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.HasIndex(e => e.SendTo, "IX_Notification_SendTo");

            entity.Property(e => e.Id).HasComment("通知訊息的編號");
            entity.Property(e => e.IsRead).HasComment("訊息為 1.已讀 或 2.未讀");
            entity.Property(e => e.KindOfNotifi).HasComment("訊息的種類是哪一種(1:帳號通知)(2:訂單通知)");
            entity.Property(e => e.LinkTo).HasComment("連結的網址(訊息內的button按鈕)");
            entity.Property(e => e.SendTime)
                .HasComment("通知發送時間")
                .HasColumnType("datetime");
            entity.Property(e => e.SendTo).HasComment("誰接收訊息(來自UserId)");
            entity.Property(e => e.Text).HasComment("訊息內文(手風琴拉開後的顯示)");
            entity.Property(e => e.Title).HasComment("訊息標題(手風琴未拉開的顯示)");

            entity.HasOne(d => d.SendToNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SendTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.HasIndex(e => e.SellerId, "IX_Order_SellerId");

            entity.HasIndex(e => e.UserId, "IX_Order_UserId");

            entity.Property(e => e.Id).HasComment("訂單編號");
            entity.Property(e => e.CouponId).HasComment("優惠券的Id，如果沒使用則為0");
            entity.Property(e => e.CreateTime)
                .HasComment("訂單下單時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Evaluate).HasComment("訂單評價，(評分範圍可為1~5)");
            entity.Property(e => e.EvaluateDate)
                .HasComment("訂單評價的時間")
                .HasColumnType("date");
            entity.Property(e => e.EvaluateText).HasComment("訂單評價的留言文字");
            entity.Property(e => e.GreenPayId).HasComment("綠界Id");
            entity.Property(e => e.Memo).HasComment("備註欄 當買家取消or退貨時 不為null");
            entity.Property(e => e.PayTime)
                .HasComment("訂單付款完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Payment).HasComment("付款方式 ( 1:轉帳、2:信用卡 )");
            entity.Property(e => e.RecipientAddress).HasComment("收件地址");
            entity.Property(e => e.RecipientCity).HasComment("收件人的城市");
            entity.Property(e => e.RecipientName).HasComment("收件人姓名");
            entity.Property(e => e.RecipientPhone).HasComment("收件人電話");
            entity.Property(e => e.RecipientPostCode).HasComment("收件人的郵遞區號");
            entity.Property(e => e.SellerId).HasComment("賣家Id ( 因為不能合併結帳，所以一張單只會有一個賣家 )");
            entity.Property(e => e.Status).HasComment("訂單狀態( 1:尚未付款 2:處理中(買家已付款，賣家未出貨) 3:帶收貨 4:已完成 5:已取消(三天內沒付款，或是買家自行取消) 6:退貨申請中 7:已退貨)");
            entity.Property(e => e.StatusCancelTime)
                .HasComment("訂單進入取消的時間")
                .HasColumnType("datetime");
            entity.Property(e => e.StatusFinishTime)
                .HasComment("訂單完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.StatusSendTime)
                .HasComment("賣家計出貨物的時間")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalPrice)
                .HasComment("訂單最終結帳金額")
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasComment("哪個帳號下訂單的");

            entity.HasOne(d => d.Seller).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Seller");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.OrderId, "IX_OrderDetail_OrderId");

            entity.HasIndex(e => e.ProductSpecificationId, "IX_OrderDetail_ProductSpecificationId");

            entity.Property(e => e.Id).HasComment("訂單明細編號");
            entity.Property(e => e.FirstSepcItem).HasComment("第一規格選項名稱(紀錄)");
            entity.Property(e => e.FirstSpecification).HasComment("第一規格標題名稱(紀錄)");
            entity.Property(e => e.OrderId).HasComment("訂單標號");
            entity.Property(e => e.ProductImg)
                .HasDefaultValueSql("(N'')")
                .HasComment("商品圖片");
            entity.Property(e => e.ProductName).HasComment("商品名稱(紀錄)");
            entity.Property(e => e.ProductSpecificationId).HasComment("商品規格Id ( 一個明細只能記錄一種商品，規格不同也算不同商品 )");
            entity.Property(e => e.Quantity).HasComment("商品個數數量");
            entity.Property(e => e.SecondSepcItem).HasComment("第二規格選項名稱(紀錄)");
            entity.Property(e => e.SecondSpecification).HasComment("第二規格標題名稱(紀錄)");
            entity.Property(e => e.SellerName).HasComment("店家名稱(紀錄)");
            entity.Property(e => e.UnitPrice)
                .HasComment("商品單數原價(紀錄)")
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.ProductSpecification).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductSpecificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_ProductSpecification");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.SellerId, "IX_Product_SellerId");

            entity.HasIndex(e => e.SubCategory, "IX_Product_SubCategory");

            entity.Property(e => e.Id).HasComment("產品編號");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasComment("商品的描述");
            entity.Property(e => e.Editor).HasComment("商品介紹，用富文本編輯器轉HTML來儲存");
            entity.Property(e => e.MadeCountry).HasComment("商品產地");
            entity.Property(e => e.Price)
                .HasComment("產品價格")
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductName).HasComment("產品名稱");
            entity.Property(e => e.ProductStatus).HasComment("1:未上架 2.已上架");
            entity.Property(e => e.SellerId).HasComment("誰賣這個商品");
            entity.Property(e => e.SubCategory).HasComment("產品小分類Id(產品只會連到小分類，小分類才會連到大分類)");
            entity.Property(e => e.ViewTimes).HasComment("總瀏覽數");

            entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Seller");

            entity.HasOne(d => d.SubCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_SubCategory");
        });

        modelBuilder.Entity<ProductImg>(entity =>
        {
            entity.ToTable("ProductImg");

            entity.HasIndex(e => e.ProductId, "IX_ProductImg_ProductId");

            entity.Property(e => e.Id).HasComment("商品圖片Id");
            entity.Property(e => e.Img).HasComment("圖片本身(網址)");
            entity.Property(e => e.ProductId).HasComment("這個圖片屬於哪個商品");
            entity.Property(e => e.Sort).HasComment("商品排序順位(同一個商品的照片排序，必須四張)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImgs)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImg_Product");
        });

        modelBuilder.Entity<ProductSpecification>(entity =>
        {
            entity.ToTable("ProductSpecification");

            entity.HasIndex(e => e.ProductId, "IX_ProductSpecification_ProductId");

            entity.Property(e => e.Id).HasComment("商品規格的Id");
            entity.Property(e => e.FirstSepcItem).HasComment("商品第一種規格的選項");
            entity.Property(e => e.FirstSpecification).HasComment("商品第一種規格名稱(標題)");
            entity.Property(e => e.ProductId).HasComment("商品的Id");
            entity.Property(e => e.SecondSepcItem).HasComment("商品第二種規格的選項");
            entity.Property(e => e.SecondSpecification).HasComment("商品第二種規格名稱(標題)");
            entity.Property(e => e.UnitPrice)
                .HasComment("這個規格如果有與Product價格不同時將會存在這個欄位")
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSpecifications)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductSpecification_Product");
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.ToTable("ProductTag");

            entity.Property(e => e.Id).HasComment("商品標籤Id");
            entity.Property(e => e.EditLastTime)
                .HasComment("資料最後更改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.TagName).HasComment("標籤名稱");
            entity.Property(e => e.UserId).HasComment("最後更改資料的");
        });

        modelBuilder.Entity<ProductTagList>(entity =>
        {
            entity.ToTable("ProductTagList");

            entity.HasIndex(e => e.ProductId, "IX_ProductTagList_ProductId");

            entity.HasIndex(e => e.ProductTagId, "IX_ProductTagList_ProductTagId");

            entity.Property(e => e.Id).HasComment("商品標籤的清單");
            entity.Property(e => e.ProductId).HasComment("商品Id");
            entity.Property(e => e.ProductTagId).HasComment("商品標籤Id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTagLists)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductTagList_Product");

            entity.HasOne(d => d.ProductTag).WithMany(p => p.ProductTagLists)
                .HasForeignKey(d => d.ProductTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductTagList_ProductTag");
        });

        modelBuilder.Entity<Reviewer>(entity =>
        {
            entity.ToTable("Reviewer");

            entity.HasIndex(e => e.AdministratorId, "IX_Reviewer_AdministratorId");

            entity.HasIndex(e => e.UserId, "IX_Reviewer_UserId");

            entity.Property(e => e.Id).HasComment("user成為賣家的審核表 ID");
            entity.Property(e => e.AdministratorId).HasComment("審核者Id");
            entity.Property(e => e.ApplyName).HasComment("申請人姓名");
            entity.Property(e => e.ApplyPhone).HasComment("申請人手機號碼");
            entity.Property(e => e.City).HasComment("商店實際所在位置(縣市)");
            entity.Property(e => e.CreateTime)
                .HasComment("申請表提交時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasComment("商店負責人的Email");
            entity.Property(e => e.FailReason).HasComment("審核失敗時，失敗的原因");
            entity.Property(e => e.IdNumber).HasComment("申請人Id");
            entity.Property(e => e.Introduction).HasComment("品牌介紹文字");
            entity.Property(e => e.LastEditTime).HasColumnType("datetime");
            entity.Property(e => e.ProductOrigin).HasComment("商品產地");
            entity.Property(e => e.ReviewTime)
                .HasComment("何時審核的")
                .HasColumnType("datetime");
            entity.Property(e => e.Reviewstatus).HasComment("審核狀態 0:未審核 1:審核失敗 2:審核成功");
            entity.Property(e => e.ShopName).HasComment("品牌名稱 (設計館名稱)");
            entity.Property(e => e.SocialMedia).HasComment("商店的社群媒體網址");
            entity.Property(e => e.UserId).HasComment("誰申請的");

            entity.HasOne(d => d.Administrator).WithMany(p => p.Reviewers)
                .HasForeignKey(d => d.AdministratorId)
                .HasConstraintName("FK_Reviewer_Administrator");

            entity.HasOne(d => d.User).WithMany(p => p.Reviewers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviewer_User");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.ToTable("Seller");

            entity.Property(e => e.Id).HasComment("商店Id");
            entity.Property(e => e.Address).HasComment("商店登記地址");
            entity.Property(e => e.ApplicantEnglishName).HasComment("商店的負責人英文姓名");
            entity.Property(e => e.ApplyName).HasComment("商店的負責人姓名");
            entity.Property(e => e.ApplyPhone).HasComment("商店的負責人電話");
            entity.Property(e => e.BankAccount).HasComment("店家收款帳號的銀行帳號");
            entity.Property(e => e.BankAccountName).HasComment("店家收款帳號的戶名");
            entity.Property(e => e.BankCode).HasComment("店家收款帳號的銀行代碼");
            entity.Property(e => e.BranchCode).HasComment("店家收款帳號的分行代碼");
            entity.Property(e => e.BrandName).HasComment("授權經銷品牌: hatsukoi");
            entity.Property(e => e.City).HasComment("商店實際所在位置(縣市)");
            entity.Property(e => e.CreateDate)
                .HasComment("商店創建日期")
                .HasColumnType("date");
            entity.Property(e => e.Currency).HasComment("使用的幣別(1:台幣 2:美金)");
            entity.Property(e => e.Email).HasComment("商店負責人的Email");
            entity.Property(e => e.Icon).HasComment("商店照片icon網址");
            entity.Property(e => e.IdNumber).HasComment("商店的負責人的身分證字號");
            entity.Property(e => e.Introduction).HasComment("品牌介紹文字");
            entity.Property(e => e.Logo).HasComment("商店照片logo網址");
            entity.Property(e => e.ModifiDate)
                .HasComment("修改商店資料時間")
                .HasColumnType("datetime");
            entity.Property(e => e.PostalCode).HasComment("商店登記地址的郵遞區號");
            entity.Property(e => e.ProductOrigin).HasComment("商品產地");
            entity.Property(e => e.SecondEmail).HasComment("商店負責人的第二Email");
            entity.Property(e => e.ShopBannerRect).HasComment("店家的設計館招牌(長方形)");
            entity.Property(e => e.ShopBannerSquare).HasComment("店家的設計館招牌(正方形)");
            entity.Property(e => e.ShopName).HasComment("商店名稱");
            entity.Property(e => e.SocialMedia).HasComment("商店的社群媒體網址");
            entity.Property(e => e.StateSecondEmail).HasComment("第二組email修改時的暫存");
            entity.Property(e => e.StateSecondTime)
                .HasComment("第二組email修改時的時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Story).HasComment("商店的故事(介紹)");
            entity.Property(e => e.TaxIdNumber).HasComment("統一編號(8碼數字)");
            entity.Property(e => e.UserId).HasComment("他的買家Id");
            entity.Property(e => e.VacationFirstDay)
                .HasComment("在休假模式時，休假的第一天")
                .HasColumnType("date");
            entity.Property(e => e.VacationLastDay)
                .HasComment("在休假模式時，休假的最後一天")
                .HasColumnType("date");
            entity.Property(e => e.VerificationCode).HasComment("修改財務信箱與收款帳戶時，需輸入驗證碼證明身分。(在成為賣家時會給)");
        });

        modelBuilder.Entity<SellerChatOnline>(entity =>
        {
            entity.ToTable("SellerChatOnline");

            entity.Property(e => e.Id).HasComment("賣家線上清單的Id");
            entity.Property(e => e.ConnectionId).HasComment("當連線進聊天室時會獲得一組連線ID");
            entity.Property(e => e.SellerId).HasComment("聊天時的Seller身分");

            entity.HasOne(d => d.Seller).WithMany(p => p.SellerChatOnlines)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK_SellerChatOnline_Seller");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.ToTable("SubCategory");

            entity.HasIndex(e => e.CategoryId, "IX_SubCategory_CategoryId");

            entity.Property(e => e.Id).HasComment("商品小分類Id");
            entity.Property(e => e.CategoryId).HasComment("商品大分類的Id");
            entity.Property(e => e.EditLastTime)
                .HasComment("資料最後更改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.SubCategoryName).HasComment("產品小分類的名稱");
            entity.Property(e => e.UserId).HasComment("最後更改資料的");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubCategory_Category");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasComment("使用者ID");
            entity.Property(e => e.Account).HasComment("帳號，(因為會有第三方登入，所以設為可為NULL)");
            entity.Property(e => e.BirthDate)
                .HasComment("使用者的生日")
                .HasColumnType("date");
            entity.Property(e => e.CertifiedTime)
                .HasComment("寄驗證信時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ChangeEmailTime)
                .HasComment("寄出修改email信時的時間記錄")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateDate)
                .HasComment("使用者加入Hatsukoi日期")
                .HasColumnType("date");
            entity.Property(e => e.Email).HasComment("使用者的信箱(第三方登入的話也會有信箱)");
            entity.Property(e => e.EmailRigisterTime).HasColumnType("datetime");
            entity.Property(e => e.Gender).HasComment("使用者的性別，0:男,1:女");
            entity.Property(e => e.Identifier).HasComment("第三方登入會有一組識別碼，有這個就不會有帳號密碼");
            entity.Property(e => e.IdentifierFb)
                .HasComment("FB第三方登入的id")
                .HasColumnName("IdentifierFB");
            entity.Property(e => e.IdentifierLine).HasComment("Line第三方登入的id");
            entity.Property(e => e.IsEmailActivity).HasComment("電子郵件 活動優惠 通知設定");
            entity.Property(e => e.IsEmailCertified).HasComment("信箱是否認證通過(第三方登入也會有信箱，會判定為已驗證)");
            entity.Property(e => e.IsEmailFocus).HasComment("電子郵件關注評價優惠券 通知設定");
            entity.Property(e => e.IsEmailFollow).HasComment("電子郵件 追蹤的商品和設計館 通知設定");
            entity.Property(e => e.IsEmailHatsukoi).HasComment("電子郵件 站內訊息 通知設定");
            entity.Property(e => e.IsEmailMember).HasComment("電子郵件 會員權益 通知設定");
            entity.Property(e => e.IsEmailOrder).HasComment("電子郵件 訂單狀態 通知設定");
            entity.Property(e => e.IsEmailPersonal).HasComment("電子郵件 個人化推薦 通知設定");
            entity.Property(e => e.IsEmailWeek).HasComment("電子郵件 會員雙周報 通知設定");
            entity.Property(e => e.LastLoginTime)
                .HasComment("最後上線時間")
                .HasColumnType("datetime");
            entity.Property(e => e.LookNotifyTime)
                .HasComment("使用者進入通知中心的時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberLevel).HasComment("會員等級，0:品藍,1:白銀,2:黃金,3:鑽石,4:尊爵");
            entity.Property(e => e.ModifiDate)
                .HasComment("修改使用者資料時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasComment("User姓名");
            entity.Property(e => e.NewEmail).HasComment("修改Email時暫存新的email");
            entity.Property(e => e.Password).HasComment("密碼，(因為會有第三方登入，所以設為可為NULL)");
            entity.Property(e => e.Phone).HasComment("使用者電話");
            entity.Property(e => e.Photo).HasComment("使用者的頭貼");
            entity.Property(e => e.ResetPasswordTime)
                .HasComment("寄重設密碼信時間")
                .HasColumnType("datetime");
            entity.Property(e => e.StoreStatus).HasComment("店家狀態,0:沒申請,1:審核中,2:駁回,3:通過,4:停權");
        });

        modelBuilder.Entity<ViewTime>(entity =>
        {
            entity.ToTable("ViewTime");

            entity.HasIndex(e => e.ProductId, "IX_ViewTime_ProductId");

            entity.HasIndex(e => e.UserId, "IX_ViewTime_UserId");

            entity.Property(e => e.Id).HasComment("觀看次數記錄的Id");
            entity.Property(e => e.ProductId).HasComment("被觀看的商品Id");
            entity.Property(e => e.Time)
                .HasComment("瀏覽的時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasComment("誰觀看這個商品");

            entity.HasOne(d => d.Product).WithMany(p => p.ViewTimesNavigation)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ViewTime_Product");

            entity.HasOne(d => d.User).WithMany(p => p.ViewTimes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ViewTime_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
