namespace Application.Books.SetBookContent
{
    public class SetBookContentResponse
    {
        public string ContentFileLocation { get; }

        public SetBookContentResponse(string contentFileLocation)
        {
            ContentFileLocation = contentFileLocation;
        }
    }
}