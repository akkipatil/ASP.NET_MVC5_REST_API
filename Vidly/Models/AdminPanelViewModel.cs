using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vidly.Models
{
    public class AdminPanelViewModel
    {
        public  List<User> UserListForAdmin { get; set; }
        public AdminUser AdminUser { get; set; }
        public User SelectedUser { get; set; }
        public List<OperationLog> OperationLogs { get; set; }

        private List<Status> StatusList = new List<Status>()
            {
                new Status() {StatusId = 1, Name = "Active"},
                new Status() {StatusId = 2, Name = "Inctive"},
                new Status() { StatusId = 3, Name = "Locked" },
                new Status() { StatusId = 4, Name = "Unlocked" },
            };

        public SelectList GetDropDownItms(short anStatusId)
        {
            return new SelectList(StatusList, "StatusId", "Name", anStatusId);
        }

    }

    public class Status
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
    }
}