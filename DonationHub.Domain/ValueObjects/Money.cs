using DonationHub.Domain.Common;

namespace DonationHub.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = default!;

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency.ToUpper();
    }


    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new ArgumentException("Currency must be a valid 3-letter code.", nameof(currency));

        return new Money(amount, currency);
    }
    protected override IEnumerable<object?> GetValues()
    {
        //بدل ما اخليهم في list بشكل يدوي بخليه يرجعهم في IEnumerable
        yield return Amount;
        yield return Currency;
    }
}