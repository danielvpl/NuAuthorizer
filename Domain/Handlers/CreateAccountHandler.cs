using Domain.Aggregates;
using Domain.Commands.Requests;
using Domain.Interfaces.Handlers;
using Domain.Responses;
using System;

namespace Domain.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, OperationResult>
    {
        private readonly NotificationContext _notification;

        public CreateAccountHandler(NotificationContext notification)
        {
            _notification = notification;
        }

        public OperationResult Handler(CreateAccountCommand command)
        {
            try
            {
                var account = new Account(command.Account.ActiveCard, command.Account.AvailableLimit);
                return new OperationResult()
                {
                    Account = account,
                    Violations = _notification.Notifications
                };
            }
            catch (Exception ex)
            {
                _notification.AddNotification(ex.Message);
            }
            return null;
        }
    }
}
