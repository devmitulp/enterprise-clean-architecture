using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.JobTitles.DTOs;
using FluentValidation;

namespace Application.Features.JobTitles.Validators
{
    public class JobTitleInputDtoValidator : BaseValidator<JobTitleInputDto>
    {
        public JobTitleInputDtoValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThanValidation(nameof(JobTitleInputDto.Id), 0, L, x => x.Id.HasValue);

            RuleFor(x => x.Name)
                .Required(nameof(JobTitleInputDto.Name), L)
                .MaxLengthValidation(nameof(JobTitleInputDto.Name), 100, L);

            RuleFor(x => x.Description!)
                .MaxLengthValidation(nameof(JobTitleInputDto.Description), 500, L)
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
