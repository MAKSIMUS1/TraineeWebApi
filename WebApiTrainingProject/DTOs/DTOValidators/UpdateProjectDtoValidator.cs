using FluentValidation;
using WebApiTrainingProject.DTOs.Request;

namespace WebApiTrainingProject.DTOs.DTOValidators
{
    public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The project name cannot be empty.");
        }
    }
}
