namespace Core.ResponseContract.Abstract;

public interface IPaginationUriDispatcher
{
    public Uri GenerateUri(IPaginationQuery query);
}