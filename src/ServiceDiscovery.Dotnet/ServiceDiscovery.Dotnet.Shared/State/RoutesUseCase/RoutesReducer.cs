using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;

public static class RoutesReducer
{
    [ReducerMethod]
    public static GatewayState ReduceIncrementCounterAction(GatewayState state, AddRouteAction action) =>
       state with { Routes = state.Routes.Append(action.RouteDto) };

    [ReducerMethod(typeof(PostNewRouteAction))]
    public static GatewayState ReducePostNewRouteAction(GatewayState state) => state;
    
    [ReducerMethod]
    public static GatewayState ReducePostNewRouteResultAction(GatewayState state, PostNewRouteResultAction action) =>
    state with
    {
        Routes = state.Routes.Append(action.Dto)
    };
}
