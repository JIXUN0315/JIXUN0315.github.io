using AdminManagement.Models.Dtos;
using AdminManagement.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Entity;
using System.Linq;
using static AdminManagement.Models.Dtos.IncreasedUserDto;

namespace AdminManagement.Services
{
    public class RevenueService
    {
        private readonly IRepository _repository;


        public RevenueService(IRepository repository)
        {
            _repository = repository;
        }

        public RevenueDto GetThisYearRevenue(int year, int month)
        {
            
            var monthRevenue = _repository.ListWhere<Order>(o => o.Status == 4 && o.StatusFinishTime.Value.Month == month && o.StatusFinishTime.Value.Year == year)
                .Sum(o => o.TotalPrice);

            var yearRevenue = _repository.ListWhere<Order>(o => o.Status == 4 && o.StatusFinishTime.Value.Year == year)
                .Sum(o => o.TotalPrice);

            var revenueDto = new RevenueDto()
            {
                Month = month,
                Year = year,
                MonthTotalRevenue = monthRevenue,
                YearTotalRevenue = yearRevenue
            };

            return revenueDto;

        }

        //求每個月營收
        public List<RevenueDto> GetRevenueByMonth(int year)
        {
            var revenueByMonth = new List<RevenueDto>();

            for (int i = 1; i <= 12; i++)
            {
                var revenue = _repository.ListWhere<Order>(o => o.Status == 4 && o.StatusFinishTime.Value.Month == i && o.StatusFinishTime.Value.Year == year)
                .Sum(o => o.TotalPrice);
                var revenueDto = new RevenueDto
                {
                    Month = i,
                    Year = year,
                    MonthTotalRevenue = revenue
                };

                revenueByMonth.Add(revenueDto);
            }

            return revenueByMonth;
        }


        public UserCountViewModel GetUserCount()
        {
            var user = new UserCountViewModel();
            var member = _repository.ListWhere<User>(u=> true).Count();
            var seller = _repository.ListWhere<Seller>(s => true).Count();
            user.MemberCount = member;
            user.SellerCount = seller;
            return user;
        }

       public IncreasedUserDto GetIncreasedUserData(int year)
        {
            var user = new IncreasedUserDto();
            var memberData = new List<MemberDto>();
            var sellerData = new List<SellerDto>();

            for (int i = 1; i <= 12; i++)
            {
                var memberCount = _repository.ListWhere<User>(u=>u.CreateDate.Year == year && u.CreateDate.Month == i).Count();
                var member = new MemberDto
                {
                    Month = i,
                    Year = year,
                    MonthMemberCount = memberCount == 0 ? 0 : memberCount
                };
                memberData.Add(member);

                var sellerCount = _repository.ListWhere<Seller>(u => u.CreateDate.Year == year && u.CreateDate.Month == i).Count();
                var seller = new SellerDto
                {
                    Month = i,
                    Year = year,
                    MonthSellerCount = sellerCount == 0 ? 0 : sellerCount
                };
                sellerData.Add(seller);
            }
            user.MemberData = memberData;
            user.SellerData = sellerData;
            return user;
        }
        

    }
}
