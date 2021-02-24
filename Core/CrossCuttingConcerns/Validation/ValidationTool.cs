using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static void Validate(IValidator validator, object entity)   //IValidator örnek olarak ProductValidator // abstract validator'ı inherit eder.
        {
            var context = new ValidationContext<Object>(entity);            
            var result =validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
