using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeenHub.Constants;
using System;
using System.Globalization;
using KeenHub.Extensions;

namespace KeenHub.Models
{
    public class Country
    {
        [Key]
        [StringLength(NumericConstants.ExactLengthCountryCode,
            MinimumLength = NumericConstants.ExactLengthCountryCode,
            ErrorMessageResourceName = "ExactFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string Code
        {
            get { return _Code; }
            set { _Code = value != null ? CultureInfo.CurrentCulture.TextInfo.ToUpper(value) : null; }
        }
        [NotMapped]
        private string _Code;
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            Country country = obj as Country;
            return country != null && Code.Equals(country.Code, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }

    [ComplexType]
    public class Address
    {
        [StringLength(NumericConstants.MaxLengthStreet,
            ErrorMessageResourceName = "MaxFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string Street1 { get; set; }
        [StringLength(NumericConstants.MaxLengthStreet,
            ErrorMessageResourceName = "MaxFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string Street2 { get; set; }
        [Index("CityIndex")]
        [Required]
        [StringLength(NumericConstants.MaxLengthCity,
            ErrorMessageResourceName = "MaxFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string City { get; set; }
        [Required]
        [StringLength(NumericConstants.ExactLengthCountryCode,
            MinimumLength = NumericConstants.ExactLengthCountryCode,
            ErrorMessageResourceName = "ExactFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string CountryCode { get; set; }
        [StringLength(NumericConstants.MaxAddressCode,
            ErrorMessageResourceName = "MaxFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string PostalCode { get; set; }
        [StringLength(NumericConstants.MaxAddressCode,
            ErrorMessageResourceName = "MaxFieldLengthIncorrect",
            ErrorMessageResourceType = typeof(Resources.Errors))]
        public string RegionCode { get; set; }

        public override bool Equals(object obj)
        {
            Address addr = obj as Address;
            return addr != null &&
                Street1.Equals(addr.Street1, StringComparison.OrdinalIgnoreCase) &&
                Street2.Equals(addr.Street2, StringComparison.OrdinalIgnoreCase) &&
                PostalCode.Equals(addr.PostalCode, StringComparison.OrdinalIgnoreCase) &&
                RegionCode.Equals(addr.RegionCode, StringComparison.OrdinalIgnoreCase) &&
                CountryCode.Equals(addr.CountryCode, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return (Street1 != null ? Separators.CompanyTagSeparator + Street1 : "") + (Street2 != null ? Separators.CompanyTagSeparator + Street2 : "") +
                (PostalCode != null ? Separators.CompanyTagSeparator + PostalCode : "") + (City != null ? Separators.CompanyTagSeparator + City : "") +
                (RegionCode != null ? Separators.CompanyTagSeparator + RegionCode : "") +
                (CountryCode != null ? Separators.CompanyTagSeparator + CountryCode : "") + "";
        }

        public override int GetHashCode()
        {
            int hash = NumericConstants.HashPrime1;
            object[] fields = { Street1, Street2, City, PostalCode, RegionCode, CountryCode };

            foreach (object field in fields)
                hash = hash * NumericConstants.HashPrime2 + (field == null ? 1 : field.GetHashCode());

            return hash;
        }
    }

    [ComplexType]
    public class Location
    {
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }

    [ComplexType]
    public class CompanyLocation
    {
        public string Description { get; set; }
        public bool Headquarters { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public Address Address { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }

    [ComplexType]
    public class ContactInfo
    {
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
    }
}