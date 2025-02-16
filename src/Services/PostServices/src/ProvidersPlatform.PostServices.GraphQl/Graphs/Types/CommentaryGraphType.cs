using GraphQL.Types;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.Types;

public sealed class CommentaryGraphType : ObjectGraphType<Commentary>
{
    public CommentaryGraphType()
    {
        Field<int>("user_id", x => x.UserId);
        Field<string?>("text" ,x => x.Text);
        Field<string?>("image_Url",x => x.ImageUrl);
        Field<int>("assesment", x => x.Assessment);
    }
}

// public partial class Commentary
// {
//     public int PostId { get; set; }
//
//     public int UserId { get; set; }
//
//     public string Text { get; set; } = null!;
//
//     public string? ImageUrl { get; set; }
//
//     public int Assessment { get; set; }
//
//     public virtual Post Post { get; set; } = null!;
//
//     public virtual User User { get; set; } = null!;
// }
