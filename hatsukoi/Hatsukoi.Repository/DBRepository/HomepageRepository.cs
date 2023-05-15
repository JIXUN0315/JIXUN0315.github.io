﻿using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository
{
    public class HomepageRepository : EFRepository, IHomepageRepository
    {
        public HomepageRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }
    }
}
