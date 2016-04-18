using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.Notifications
{
    public class Notification
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public NotificationType Type { get; set; }

        public Notification(string title, string description, NotificationType type)
        {
            Title = title;
            Description = description;
            Type = type;

        }
 
        public string getAlertClass()
        {
            string alert = "";

            switch (Type)
            {
                case NotificationType.NORMAL:
                    alert = "alert-info";
                    break;
                case NotificationType.WARNING:
                    alert = "alert-warning";
                    break;
                case NotificationType.SUCCESS:
                    alert = "alert-success";
                    break;
                case NotificationType.ERROR:
                    alert = "alert-danger";
                    break;
            }

            return alert;
        }
    }
}