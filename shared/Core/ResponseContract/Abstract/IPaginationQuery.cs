namespace Core.ResponseContract.Abstract;

public interface IPaginationQuery
{
    int PageNumber { get; }
    int PageSize { get; }
}