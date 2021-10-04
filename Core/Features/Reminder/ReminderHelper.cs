using Plugin.LocalNotification;
using System;
using System.Drawing;
using Xamarin.Forms;

namespace Core
{
    public static class ReminderHelper
    {
        public static void Upsert(int id, int index, string title, string message, DateTime dateTime)
        {
            var androidOptions = new AndroidOptions { IconName = "ic_notification", AutoCancel = true};
            var iosOptions = new iOSOptions { HideForegroundAlert = false };
            var notification = new NotificationRequest
            {
                NotificationId = id,
                Title = $"{title}",
                Android = androidOptions,
                BadgeNumber = index,
                Description = $"{message}",
                ReturningData = $"{index}",
                Sound = Device.RuntimePlatform == Device.Android ? "bell" : "bell.aiff",
                NotifyTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0)
        };
            NotificationCenter.Current.Show(notification);
            StoreService.Instance.GetCollection<Reminder>().Upsert(new Reminder { Id = id.ToString(), DateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0).ToLocalTime() });
        }

        public static void UpdateHour(DateTime timeSpan)
        {
            var reminders = StoreService.Instance.GetCollection<Reminder>().FindAll();

            foreach (var reminder in reminders)
            {
                reminder.DateTime = new DateTime(reminder.DateTime.Year, reminder.DateTime.Month, reminder.DateTime.Hour, timeSpan.Hour, timeSpan.Minute, 0).ToLocalTime();
                StoreService.Instance.GetCollection<Reminder>().Upsert(reminder);
            }
        }
    }
}
