using System.Diagnostics.CodeAnalysis;

namespace NetworkScannerXTech.Shared;
public class Result<T>
{
    public T? Value { get; }
    public string Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    private Result(bool isSuccess, T? value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    public static Result<T> Failure(string error) => new(false, default, error);
}
