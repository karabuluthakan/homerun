namespace Core.ResponseContract.Abstract;

public interface IResponse
{
    public string Detail { get; }
    public ResponseReason Reason { get; }
    public bool Success { get; }
    public IDictionary<string, object> Extensions { get; }
}