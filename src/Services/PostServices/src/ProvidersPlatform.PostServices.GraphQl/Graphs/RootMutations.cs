using GraphQL.Resolvers;
using GraphQL.Types;
using ProvidersPlatform.PostServices.GraphQl.Graphs.Mutations;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs;

public sealed class RootMutations : ObjectGraphType<object>
{
    public RootMutations()
    {
        AddField(new FieldType()
        {
            Name = "PostMutations",
            Type = typeof(PostMutations),
            Resolver = new FuncFieldResolver<object>(ctx => ctx)
        });
    }
}