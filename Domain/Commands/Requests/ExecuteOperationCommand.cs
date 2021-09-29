using System.Collections.Generic;

namespace Domain.Commands.Requests
{
    public class ExecuteOperationCommand
    {
        public IList<string> Commands { get; set; }        
    }
}