namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; }
    
    private OrderName(string value) => Value = value;

    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        // eger bastaki gibi degilse verilen uzunluk fırlar
        // ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength); -- sorun çıkartcak bize 5
        
        return new OrderName(value);
    }
}