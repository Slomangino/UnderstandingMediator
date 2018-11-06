using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingMediator.Validator
{
    public class TestMessageValidator : IValidator<ValidatedMessage, bool>
    {
        public Task<string> Validate(ValidatedMessage message)
        {
            var validationResult = string.Empty;

            if(message.value > 20)
            {
                validationResult += "Value was greater 20, that is not allowed.";
            }

            return Task.FromResult(validationResult);
        }
    }
}
