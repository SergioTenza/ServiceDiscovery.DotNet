using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;

public static class GatewayReducer
{
    [ReducerMethod(typeof(FetchGatewayAction))]
    public static GatewayState ReduceFetchGatewayAction(GatewayState state) => state with
    {
        IsLoading = true,
        Clusters = [],
        Routes = []
    };
    [ReducerMethod]
    public static GatewayState ReduceFetchGatewayResultAction(GatewayState state, FetchGatewayResultAction action) =>
    state with
    {
        IsLoading = false,
        Clusters = action.Clusters,
        Routes = action.Routes
    };
}
