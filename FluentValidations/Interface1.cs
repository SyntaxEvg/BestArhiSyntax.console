using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidations
{
    public abstract class BaseFluentValidation<T> : AbstractValidator<T>
    {

    }


}

//public sealed class CreateCompanyCommandValidator :
//AbstractValidator<CreateCompanyCommand>
//{
//375
//public CreateCompanyCommandValidator()
//    {
//        RuleFor(c => c.Company.Name).NotEmpty().MaximumLength(60);
//        RuleFor(c => c.Company.Address).NotEmpty().MaximumLength(60);
//    }
//}
