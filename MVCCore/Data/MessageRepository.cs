using MVCCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCore.Data
{
    public class MessageRepository
    {
        private ApplicationDbContext dbContext { get; set; }

        public MessageRepository(ApplicationDbContext dbCont)
        {
            dbContext = dbCont;
        }

        public void Add(Message msg)
        {
            dbContext.Add(msg);
            dbContext.SaveChanges();
        }

        public void Update(Message msg)
        {
            dbContext.Update(msg);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            dbContext.Message.Remove(dbContext.Message.Where(x => x.MessageID == id).FirstOrDefault());
            dbContext.SaveChanges();
        }

        public IEnumerable<MessageViewModel> GetAll()
        {
            return dbContext.Message.AsEnumerable().Select(x => new MessageViewModel()
            {
                Content = x.Content,
                Title = x.Title,
                Email = x.Email,
                MessageID = x.MessageID,
                IsUser = string.IsNullOrEmpty(x.UserID) ? false : true
            });
        }

        public IEnumerable<MessageViewModel> GetByEmail(string email)
        {
            ApplicationUser appUser = dbContext.Users.Where(x => x.Email == email).FirstOrDefault() ??
                new ApplicationUser { Id = "" };
            return dbContext.Message.Where(x => x.Email == email || x.UserID == appUser.Id).Select(x => new MessageViewModel()
            {
                Content = x.Content,
                Title = x.Title,
                Email = x.Email,
                MessageID = x.MessageID,
                IsUser = string.IsNullOrEmpty(x.UserID) ? false : true
            });
        }

        public MessageViewModel GetItem(int id)
        {
            return dbContext.Message.Where(x => x.MessageID == id).Select(x => new MessageViewModel()
            {
                Content = x.Content,
                Title = x.Title,
                Email = x.Email,
                MessageID = x.MessageID,
                IsUser = string.IsNullOrEmpty(x.UserID) ? false : true
            }).FirstOrDefault();
        }
    }
}
