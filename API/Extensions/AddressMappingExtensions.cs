using API.DTOs;
using Core.Entities;

namespace API.Extensions;

public static class AddressMappingExtensions
{
    public static AddressDTO? ToAddressDTO(this Address address)
    {
        return address == null
            ? null
            : new AddressDTO
            {
                StreetAddress = address.StreetAddress,
                Ward = address.Ward,
                District = address.District,
                Province = address.Province,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
    }

    public static Address? ToEntity(this AddressDTO addressDto)
    {
        return addressDto == null
            ? null
            : new Address
            {
                StreetAddress = addressDto.StreetAddress,
                Ward = addressDto.Ward,
                District = addressDto.District,
                Province = addressDto.Province,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country
            };
    }

    public static void UpdateFromDTO(this Address address, AddressDTO addressDto)
    {

        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(addressDto);
        address.StreetAddress = addressDto.StreetAddress;
        address.Ward = addressDto.Ward;
        address.District = addressDto.District;
        address.Province = addressDto.Province;
        address.PostalCode = addressDto.PostalCode;
        address.Country = addressDto.Country;
    }
}
