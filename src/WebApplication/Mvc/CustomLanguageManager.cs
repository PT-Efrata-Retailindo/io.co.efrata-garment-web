using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Mvc
{
    public class CustomLanguageManager : FluentValidation.Resources.LanguageManager
    {
        public CustomLanguageManager()
        {
            AddTranslation("id", "EmailValidator", "'{PropertyName}' bukan alamat email yang benar.");
            AddTranslation("id", "GreaterThanOrEqualValidator", "'{PropertyName}' harus lebih besar dari atau sama dengan '{ComparisonValue}'.");
            AddTranslation("id", "GreaterThanValidator", "'{PropertyName}' harus lebih besar dari '{ComparisonValue}'.");
            AddTranslation("id", "LengthValidator", "'{PropertyName}' harus antara {MinLength} dan {MaxLength} karakter. Anda menulis {TotalLength} karakter.");
            AddTranslation("id", "MinimumLengthValidator", "Panjang dari '{PropertyName}' harus minimal {MinLength} karakter. Anda menulis {TotalLength} karakter.");
            AddTranslation("id", "MaximumLengthValidator", "Panjang dari '{PropertyName}' harus {MaxLength} karakter lebih sedikit. Anda menulis {TotalLength} karakter.");
            AddTranslation("id", "LessThanOrEqualValidator", "'{PropertyName}' harus kurang dari atau sama dengan '{ComparisonValue}'.");
            AddTranslation("id", "LessThanValidator", "'{PropertyName}' harus kurang dari '{ComparisonValue}'.");
            AddTranslation("id", "NotEmptyValidator", "'{PropertyName}' tidak boleh kosong.");
            AddTranslation("id", "NotEqualValidator", "'{PropertyName}' tidak boleh sama dengan '{ComparisonValue}'.");
            AddTranslation("id", "NotNullValidator", "'{PropertyName}' tidak boleh kosong.");
            AddTranslation("id", "PredicateValidator", "The specified condition was not met for '{PropertyName}'.");
            AddTranslation("id", "AsyncPredicateValidator", "The specified condition was not met for '{PropertyName}'.");
            AddTranslation("id", "RegularExpressionValidator", "'{PropertyName}' format tidak sesuai.");
            AddTranslation("id", "EqualValidator", "'{PropertyName}' harus sama dengan '{ComparisonValue}'.");
            AddTranslation("id", "ExactLengthValidator", "Panjang '{PropertyName}' harus {MaxLength} karakter. Anda menulis {TotalLength} karakter.");
            AddTranslation("id", "InclusiveBetweenValidator", "'{PropertyName}' must be between {From} and {To}. You entered {Value}.");
            AddTranslation("id", "ExclusiveBetweenValidator", "'{PropertyName}' must be between {From} and {To} (exclusive). You entered {Value}.");
            AddTranslation("id", "CreditCardValidator", "'{PropertyName}' bukan nomor kartu kredit yang benar.");
            AddTranslation("id", "ScalePrecisionValidator", "'{PropertyName}' must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found.");
            AddTranslation("id", "EmptyValidator", "'{PropertyName}' tidak boleh kosong.");
            AddTranslation("id", "NullValidator", "'{PropertyName}' tidak boleh kosong.");
            AddTranslation("id", "EnumValidator", "'{PropertyName}' has a range of values which does not include '{PropertyValue}'.");
        }
    }
}
