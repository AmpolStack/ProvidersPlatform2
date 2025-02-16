using GraphQL.Resolvers;
using GraphQL.Types;
using ProvidersPlatform.PostServices.GraphQl.Graphs.Queries;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs;

public sealed class RootQuery : ObjectGraphType<object>
{
    public RootQuery()
    {
        AddField(new FieldType()
        {
            Name = "PostQueries",
            Type = typeof(PostQueries),
            Resolver = new FuncFieldResolver<object>(ctx => ctx)
        });
    }
}