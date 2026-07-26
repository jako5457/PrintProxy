namespace PrintProxy.Hub.Services.Tags
{
    public interface ITagService
    {
        Task CreateTagAsync(string TagName, bool SytemTag = false);
    }
}