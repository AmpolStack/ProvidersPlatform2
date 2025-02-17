using GraphQL.Types;

namespace ProvidersPlatform.PostServices.GraphQl.Graphs;

public class DefaultSchema : Schema
{
    public DefaultSchema(RootQuery query, RootMutations mutations)
    {
        Query = query;
        Mutation = mutations;
    }
}
