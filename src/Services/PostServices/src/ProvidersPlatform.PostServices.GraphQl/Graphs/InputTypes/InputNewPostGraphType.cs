using GraphQL.Types;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.InputTypes;

public sealed class InputNewPostGraphType : InputObjectGraphType<Post>
{
    public InputNewPostGraphType()
    {
        Name = "InputNewPostType";
        Field<string?>(x => x.ImageUrl, nullable: true);
        Field<string>(x => x.Title, nullable: false);
        Field<string>(x => x.Description, nullable: false);
        Field<int>(x => x.ProviderId, nullable: false);
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