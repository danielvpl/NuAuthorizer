using Domain.Commands.Requests;
using Domain.Interfaces.Handlers;
using Domain.Responses;
using System;

namespace Domain.Handlers
{
    public class AuthorizeTransactionHandler : IRequestHandler<AuthorizeTransactionCommand, OperationResult>
    {
        private readonly NotificationContext _notification;
        
        public AuthorizeTransactionHandler(NotificationContext notification)
        {
            _notification = notification;
            _notification.Notifications.Clear();
        }

        public OperationResult Handler(AuthorizeTransactionCommand command)
        {
            try
            {
                command.Account.AvailableLimit = command.Account.AvailableLimit - command.Transaction.Amount;
                
                return new OperationResult()
                {
                    Account = command.Account,
                    Violations = _notification.Notifications
                };

            }
            catch(Exception ex)
            {
                _notification.AddNotification(ex.Message);
            }
            return null;            
        }
    }
}
