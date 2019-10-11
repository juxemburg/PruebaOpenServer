using System;
using System.Collections.Generic;
using System.Text;

namespace PokeServices.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationErrorStatus Status { get; private set; }
        public OperationFailedException(string message, OperationErrorStatus status)
            : base(message)
        {
            Status = status;
        }
    }
}
