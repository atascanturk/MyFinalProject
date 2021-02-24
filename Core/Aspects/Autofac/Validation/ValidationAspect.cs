using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception //Aspect
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType) // attributeler type ile gönderilir.
        {
            //defensive coding
            if (!typeof(IValidator).IsAssignableFrom(validatorType)) //validatortype IValidator mı? 
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil.");//değilse
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; //validater type'ın base'ini (AbstractValidator<T>) al ve genericteki ilk entityyi al.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);//methodun parametrelerine bak
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
