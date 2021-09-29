using System.Collections.Generic;
using System.Linq;

namespace Domain
{
	public class NotificationContext
	{
		private readonly List<string> _notifications;
		public List<string> Notifications => _notifications;
		public bool HasNotifications => _notifications.Any();

		public NotificationContext()
		{
			_notifications = new List<string>();
		}

		public void AddNotification(string message)
		{
			if(!_notifications.Contains(message))
				_notifications.Add(message);
		}

		public void AddNotifications(IReadOnlyCollection<string> notifications)
		{
			_notifications.AddRange(notifications);
		}
	}
}
