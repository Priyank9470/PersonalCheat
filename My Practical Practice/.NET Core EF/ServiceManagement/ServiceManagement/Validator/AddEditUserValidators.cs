using FluentValidation;
using ServiceManagement.Core.RequestModel;

namespace ServiceManagement.Validator
{
	public class AddEditUserValidators : AbstractValidator<UserRequest>
	{
		public AddEditUserValidators()
		{
			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("UserName is required.")
				.MaximumLength(50).WithMessage("UserName cannot exceed 50 characters.")
				.MinimumLength(2).WithMessage("UserName must be at least 2 characters long.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.Length(8, 12).WithMessage("Password must be between 8 and 12 characters long.");
		}
	}
}
