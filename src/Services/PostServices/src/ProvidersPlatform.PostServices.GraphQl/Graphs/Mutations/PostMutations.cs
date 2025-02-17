using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.PostServices.GraphQl.Graphs.InputTypes;
using ProvidersPlatform.PostServices.GraphQl.Graphs.ObjectTypes;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Defaults;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs.Mutations;

public sealed class PostMutations : ObjectGraphType
{
    public PostMutations()
    {
        Field<PostGraphType>("UpdatePost")
            .Arguments(new QueryArgument[]
            {
                new QueryArgument<NonNullGraphType<IntGraphType>>()
                {
                    Name = "id"
                },
                new QueryArgument<NonNullGraphType<InputPostGraphType>>()
                {
                    Name = "object"
                }
            })
            .ResolveAsync(async ctx =>
            {
                var repo = ctx.RequestServices!.GetRequiredService<IGenericRepository<Post>>();
                var id = ctx.GetArgument<int?>("id");
                if (id is null)
                {
                    throw new NullReferenceException("Id cannot be null");
                }
                
                var userFind = await repo.GetEntity(x => x.PostId == id);

                if (userFind is null)
                {
                    throw new NullReferenceException("User not found");
                }
                
                var objectFind = ctx.GetArgument<Post>("object");
                if (!string.IsNullOrEmpty(objectFind.Description))
                {
                    userFind.Description = objectFind.Description;
                }

                if(!string.IsNullOrEmpty(objectFind.Title))
                {
                    userFind.Title = objectFind.Title;
                }

                if (objectFind.ImageUrl is not null)
                {
                    userFind.ImageUrl = objectFind.ImageUrl;
                }

                var change = await repo.Update(userFind);
                if (change is false)
                {
                    throw new NullReferenceException("no se pudo papá");
                }
                
                return userFind;
            });

        Field<BooleanGraphType>("DeletePost")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async ctx =>
            {
                var repoCom = ctx.RequestServices!.GetRequiredService<IGenericRepository<Commentary>>();
                var repoPos = ctx.RequestServices!.GetRequiredService<IGenericRepository<Post>>();

                var id = ctx.GetArgument<int>("id");
                try
                {
                    var entityFind = await repoCom.GetEntities(x => x.PostId == id);
                    var commentaries = await entityFind.Include(x => x.Post).ToListAsync();
                    foreach (var com in commentaries)
                    {
                        await repoCom.Delete(com);
                    }

                    var pos = await repoPos.GetEntity(x => x.PostId == id);
                    var response = await repoPos.Delete(pos);
                    return response;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    throw new NullReferenceException("no se pudo eliminar", ex);
                }
                
            });
    }
    
}