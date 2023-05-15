using Hatsukoi.Models.Dtos.Report;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using MimeKit.Encodings;
using System.Linq;
using static Hatsukoi.Models.ViewModels.ReportViewModel;

namespace Hatsukoi.Service
{
    public class ReportService
    {
        private readonly IRepository _reportRepository;
        private readonly AccountService _accountService;
        public ReportService(IRepository reportRepository, AccountService accountService)
        {
            _reportRepository = reportRepository;
            _accountService = accountService;
        }

        public ReportViewModel StarSale()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var nowMonthOrder = _reportRepository.ListWhere<Order>(x => x.SellerId == seller.Id && x.StatusFinishTime > startMonth && x.StatusFinishTime < endMonth && x.StatusFinishTime != null)
                                             .GroupBy(x => ((DateTime)x.StatusFinishTime).ToString("yyyy-MM-dd"))
                                             .Select(g => new
                                             {
                                                 Date = g.Key,
                                                 TotalPrice = g.Sum(x => x.TotalPrice)
                                             })
                                             .OrderBy(x => x.Date);


            List<string?> nowMonthDate = new List<string?>();
            List<decimal?> nowMonthPrice = new List<decimal?>();

            foreach (var order in nowMonthOrder)
            {
                nowMonthDate.Add(order.Date);
                nowMonthPrice.Add(order.TotalPrice);
            }

            return new ReportViewModel
            {
                Date = nowMonthDate,
                Price = nowMonthPrice,
            };
        }
        public ReportViewModel NowMonthSale(ChartDto dto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var LastYearFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
            var LastYearLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1);
            var nowMonthOrder = _reportRepository.ListWhere<Order>(x => x.SellerId == seller.Id && x.StatusFinishTime > DateTime.Parse(dto.First) && x.StatusFinishTime < DateTime.Parse(dto.Last) && x.StatusFinishTime != null)
                                             .GroupBy(x => ((DateTime)x.StatusFinishTime).ToString("yyyy-MM-dd"))
                                             .Select(g => new
                                             {
                                                 Date = g.Key,
                                                 TotalPrice = g.Sum(x => x.TotalPrice)
                                             })
                                             .OrderBy(x => x.Date);


            List<string?> nowMonthDate = new List<string?>();
            List<decimal?> nowMonthPrice = new List<decimal?>();
            foreach (var order in nowMonthOrder)
            {
                nowMonthDate.Add(order.Date);
                nowMonthPrice.Add(order.TotalPrice);
            }

            return new ReportViewModel
            {
                Date = nowMonthDate,
                Price = nowMonthPrice,
            };
        }

        public ReportViewModel Order()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.Seller == seller);
            decimal? price = 0;

            foreach (var aa in haveOrder)
            {
                if (aa.StatusCancelTime != null && aa.PayTime != null && aa.StatusFinishTime != null) { }
                else if (aa.StatusCancelTime != null && aa.PayTime != null) { }
                else if (aa.PayTime != null && aa.StatusFinishTime != null) { }
                else
                {
                    //抓到只有付款完成沒有取消的，在判斷是不是當月的
                    if (aa.PayTime > startMonth && aa.PayTime < endMonth)
                    {
                        price = aa.TotalPrice;
                    }
                }
            }
            return new ReportViewModel
            {
                NotShipPrice = price
            };
        }

        public ReportViewModel ShipOrder()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.Seller == seller);
            decimal price = 0;
            foreach (var order in haveOrder)
            {
                if (order.PayTime != null && order.StatusSendTime != null)
                {
                    if (order.StatusSendTime > startMonth && order.StatusSendTime < endMonth)
                    {
                        price = order.TotalPrice;
                    }
                }
            }
            return new ReportViewModel
            {
                ShipPrice = price
            };
        }

        public ReportViewModel TotalOrder()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.Seller == seller);
            decimal price = 0;
            foreach (var order in haveOrder)
            {
                var time = order.StatusFinishTime;
                if (time != null)
                {
                    price += order.TotalPrice;
                }

            }
            return new ReportViewModel { TotalPrice = price };
        }

        public ReportViewModel OrderCount(ChartDto dto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.SellerId == seller.Id && x.StatusFinishTime > DateTime.Parse(dto.First) && x.StatusFinishTime < DateTime.Parse(dto.Last) && x.StatusFinishTime != null)
                                                         .GroupBy(x => ((DateTime)x.StatusFinishTime).ToString("yyyy-MM-dd"))
                                                         .Select(g => new
                                                         {
                                                             Date = g.Key,
                                                             Count = g.Count(),

                                                         })
                                                         .OrderBy(x => x.Date);


            List<string?> strings = new List<string?>();
            List<int?> count = new List<int?>();

            foreach (var order in haveOrder)
            {
                strings.Add(order.Date);
                count.Add(order.Count);
            }

            return new ReportViewModel
            {
                Date = strings,
                Count = count,
            };

        }

        public ReportViewModel StarOrderCount()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.SellerId == seller.Id && x.StatusFinishTime > startMonth && x.StatusFinishTime < endMonth && x.StatusFinishTime != null)
                                                         .GroupBy(x => ((DateTime)x.StatusFinishTime).ToString("yyyy-MM-dd"))
                                                         .Select(g => new
                                                         {
                                                             Date = g.Key,
                                                             Count = g.Count(),

                                                         })
                                                         .OrderBy(x => x.Date);


            List<string?> strings = new List<string?>();
            List<int?> count = new List<int?>();

            foreach (var order in haveOrder)
            {
                strings.Add(order.Date);
                count.Add(order.Count);
            }

            return new ReportViewModel
            {
                Date = strings,
                Count = count,
            };

        }

        public ReportViewModel ProductViews()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var product = _reportRepository.ListWhere<Product>(x => x.SellerId == seller.Id);

            List<string?> strings = new List<string?>();
            List<int?> views = new List<int?>();


            var aa = _reportRepository.ListWhere<Product>(x => x.SellerId == seller.Id && x.ViewTimes != 0).OrderByDescending(x => x.ViewTimes).Select(x => x.ProductName).Take(5);
            foreach (var b in aa)
            {
                strings.Add(b);

            }
            var cc = _reportRepository.ListWhere<Product>(x => x.SellerId == seller.Id && x.ViewTimes != 0).OrderByDescending(x => x.ViewTimes).Select(x => x.ViewTimes).Take(5);
            foreach (var b in cc)
            {
                views.Add(b);
            }



            return new ReportViewModel
            {
                Date = strings,
                Count = views,
            };
        }

        public ReportViewModel StarCancelOrderCount()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.SellerId == seller.Id && x.StatusCancelTime > startMonth && x.StatusCancelTime < endMonth && x.StatusCancelTime != null)
                                                         .GroupBy(x => ((DateTime)x.StatusCancelTime).ToString("yyyy-MM-dd"))
                                                         .Select(g => new
                                                         {
                                                             Date = g.Key,
                                                             Count = g.Count(),

                                                         })
                                                         .OrderBy(x => x.Date);


            List<string?> strings = new List<string?>();
            List<int?> count = new List<int?>();

            foreach (var order in haveOrder)
            {
                strings.Add(order.Date);
                count.Add(order.Count);
            }

            return new ReportViewModel
            {
                Date = strings,
                Count = count,
            };
        }

        public ReportViewModel CancelOrderCount(ChartDto dto)
        {
            var userId = _accountService.GetUser().Id;
            var seller = _reportRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);
            var haveOrder = _reportRepository.ListWhere<Order>(x => x.SellerId == seller.Id && x.StatusCancelTime > DateTime.Parse(dto.First) && x.StatusCancelTime < DateTime.Parse(dto.Last) && x.StatusCancelTime != null)
                                                         .GroupBy(x => ((DateTime)x.StatusCancelTime).ToString("yyyy-MM-dd"))
                                                         .Select(g => new
                                                         {
                                                             Date = g.Key,
                                                             Count = g.Count(),

                                                         })
                                                         .OrderBy(x => x.Date);


            List<string?> strings = new List<string?>();
            List<int?> count = new List<int?>();

            foreach (var order in haveOrder)
            {
                strings.Add(order.Date);
                count.Add(order.Count);
            }

            return new ReportViewModel
            {
                Date = strings,
                Count = count,
            };
        }
    }
}

