using Microsoft.AspNetCore.Mvc;
using MVCCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCore.ViewComponents
{
    //[ViewComponent(Name = "LatestNews")]
    public class NewsViewComponent : ViewComponent
    {
        private ApplicationDbContext dbContext;

        public NewsViewComponent(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public Task<IViewComponentResult> InvokeAsync(int count)
        {
            var items = dbContext.Message.Take(count);
            return Task.FromResult<IViewComponentResult>(View(items));
        }
    }
}
