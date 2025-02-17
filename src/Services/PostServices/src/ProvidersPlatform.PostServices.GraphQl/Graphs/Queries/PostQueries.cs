using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.PostServices.GraphQl.Graphs.ObjectTypes;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Defaults;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.Queries;

public sealed class PostQueries : ObjectGraphType
{
    public PostQueries()
    {

        Field<PostGraphType>("getPostById")
            .Argument<NonNullGraphType<IntGraphType>>("postId")
            .ResolveAsync(async ctx =>
            {
                var repository = ctx.RequestServices!.GetRequiredService<IGenericRepository<Post>>();
                var id = ctx.GetArgument<int>("postId");
                var findPost = await repository.GetEntities(x => x.PostId == id);
                var response = await findPost.Include(x => x.Commentaries).FirstAsync();
                return response;
            });
            
            
        Field<ListGraphType<PostGraphType>>("getPosts")
            .ResolveAsync(async ctx =>
            {
                var repository = ctx.RequestServices!.GetRequiredService<IGenericRepository<Post>>();
                var posts = await repository.GetEntities();
                var containsCommentary = ctx.SubFields?.Keys.Contains("commentaries");
                if (containsCommentary is false or null)
                {
                    return await posts.ToListAsync();
                }
                // var fields = ctx.GetDirective();
                return await posts.Include(x => x.Commentaries).ToListAsync();
            });

        Field<BooleanGraphType>("Healthy")
            .ResolveAsync(async ctx => await Task.FromResult(true));
    }
}