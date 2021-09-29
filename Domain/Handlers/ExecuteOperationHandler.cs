using Domain.Aggregates;
using Domain.Commands.Requests;
using Domain.Interfaces.Handlers;
using Domain.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Handlers
{
    public class ExecuteOperationHandler : IRequestHandler<ExecuteOperationCommand, string>
    {
        private readonly NotificationContext _notification;
        //Commands
        private readonly IRequestHandler<CreateAccountCommand, OperationResult> _createAccountHandler;
        private readonly IRequestHandler<AuthorizeTransactionCommand, OperationResult> _authorizeTransactionHandler;

        private Account currentAccount;
        private List<AuthorizeTransactionCommand> transactions = new List<AuthorizeTransactionCommand>();
        int countTransactions = 0;

        public ExecuteOperationHandler(NotificationContext notification,
            IRequestHandler<CreateAccountCommand, OperationResult> createAccountHandler,
            IRequestHandler<AuthorizeTransactionCommand, OperationResult> authorizeTransactionHandler)
        {
            _createAccountHandler = createAccountHandler;
            _authorizeTransactionHandler = authorizeTransactionHandler;
            _notification = notification;
        }

        public string Handler(ExecuteOperationCommand command)
        {
            StringBuilder commandOutput = new StringBuilder();
            try
            {
                foreach (var cmd in command.Commands)
                {
                    if (cmd.Contains("account"))
                    {
                        CreateAccountCommand createAccountCommand = JsonConvert.DeserializeObject<CreateAccountCommand>(cmd);
                        OperationResult output = _createAccountHandler.Handler(createAccountCommand);
                        if (currentAccount != null)
                        {
                            _notification.AddNotification("account-already-initialized");
                        }
                        commandOutput.AppendLine(JsonConvert.SerializeObject(output));
                        currentAccount = output.Account;
                    }
                    if (cmd.Contains("transaction"))
                    {
                        AuthorizeTransactionCommand authorizeTransaction = JsonConvert.DeserializeObject<AuthorizeTransactionCommand>(cmd);
                        authorizeTransaction.Account = currentAccount;

                        if (ValidateOperations(authorizeTransaction))
                        {
                            OperationResult output = _authorizeTransactionHandler.Handler(authorizeTransaction);
                            commandOutput.AppendLine(JsonConvert.SerializeObject(output));
                            transactions.Add(authorizeTransaction);                            
                        }
                        else{
                            var result = new OperationResult()
                            {
                                Account = currentAccount,
                                Violations = _notification.Notifications
                            };
                            commandOutput.AppendLine(JsonConvert.SerializeObject(result));
                            _notification.Notifications.Clear();
                        } 
                    }
                }
                return commandOutput.ToString();
            }
            catch (Exception ex)
            {
                _notification.AddNotification(ex.Message);
            }

            return null;
        } 
        
        private bool ValidateOperations(AuthorizeTransactionCommand transaction)
        {
            if (transaction.Account != null)
            {
                if (transaction.Account.ActiveCard)
                {
                    if (transaction.Transaction.Amount > transaction.Account.AvailableLimit)
                        _notification.AddNotification("insufficient-limit");
                }
                else
                {
                    _notification.AddNotification("card-not-active");
                }
            }
            else
            {
                transaction.Account = null;
                _notification.AddNotification("account-not-initialized");
            }

            if (transactions.Count > 0) {
                var lastTransaction = transactions.OrderByDescending(x => x.Transaction.Time).FirstOrDefault();
                if ((transaction.Transaction.Time - lastTransaction.Transaction.Time).TotalMinutes < 2)
                {
                    countTransactions++;

                    var duplicated = transactions.Where(x => x.Transaction.Amount == transaction.Transaction.Amount
                    && x.Transaction.Merchant.Equals(transaction.Transaction.Merchant)).FirstOrDefault();

                    if (duplicated != null)
                    {
                        _notification.AddNotification("doubled-transaction");
                        countTransactions--;
                    }
                }
                else
                {
                    countTransactions = 0;
                }
            }

            if (countTransactions >= 3)
            {
                _notification.AddNotification("high-frequency-small-interval");                
            }

            return _notification.Notifications.Count == 0;
        }
    }
}
