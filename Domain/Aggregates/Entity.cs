using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Aggregates
{
    public abstract class Entity
    {
        public Entity()
        {
            _notificationContext = new NotificationContext();
        }

        [NotMapped]
        protected NotificationContext _notificationContext;
        
        public void AddNotification(string message)
        {
            _notificationContext.AddNotification(message);
        }        
    }
}
