using GraphQL.Types;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.ObjectTypes;

public sealed class PostGraphType : ObjectGraphType<Post>
{
    public PostGraphType()
    {
        Field<int>(x => x.PostId);
        Field<int>(x => x.ProviderId);
        Field<string?>(x => x.ImageUrl);
        Field<string?>(x => x.Title);
        Field<string?>(x => x.Description);
        Field<ListGraphType<CommentaryGraphType>>("Commentaries");
    }
}

