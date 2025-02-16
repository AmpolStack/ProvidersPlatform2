using GraphQL.Types;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.Types;

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


// public int PostId { get; set; }
//
// public int ProviderId { get; set; }
//
// public string? ImageUrl { get; set; }
//
// public string Title { get; set; } = null!;
//
// public string Description { get; set; } = null!;
//
// public virtual ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
//
// public virtual Provider Provider { get; set; } = null!;