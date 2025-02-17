using GraphQL.Types;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.InputTypes;

public sealed class InputPostGraphType : InputObjectGraphType<Post>
{
    public InputPostGraphType()
    {
        Name = "InputPostType";
        Field<string?>(x => x.ImageUrl, nullable: true);
        Field<string>(x => x.Title, nullable: true);
        Field<string?>(x => x.Description, nullable: true);
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