using System;

namespace HelpLine.BuildingBlocks.Domain
{
    public class BusinessRuleValidationException : Exception
    {
        public IBusinessRuleBase BrokenRule { get; }

        public string Details { get; }

        public BusinessRuleValidationException(IBusinessRuleBase brokenRule) : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
            Details = brokenRule.Message;
        }

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
        }
    }
}
