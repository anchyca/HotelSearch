using FluentValidation;
using HotelSearchService.DTOs;

namespace HotelSearchService.Validators
{
    public class UpdateHotelDtoValidator : AbstractValidator<UpdateHotelDto>
    {
        public UpdateHotelDtoValidator()
        {
            Include(new CreateHotelDtoValidator());
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
