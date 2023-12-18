using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;

public static class Reducer
{
  // [ReducerMethod]
  // public static RoutesState ReduceIncrementCounterAction(RoutesState state, AddRouteAction action) =>
  //  state with { Routes = state.Routes.Append(action.RouteDto) };
   
  // [ReducerMethod(typeof(FetchRoutesAction))]
  // public static RoutesState ReduceFetchDataAction(RoutesState state) => state with
  // {
  //   IsLoading = true,
  //   Routes = []
  // };
  // [ReducerMethod]
  // public static RoutesState ReduceFetchRoutesResultAction(RoutesState state, FetchRoutesResultAction action) =>
  // state with
  // {
  //   IsLoading = false,
  //   Routes = action.Routes
  // };
  // [ReducerMethod(typeof(FetchClustersAction))]
  // public static ClustersState ReduceFetchDataAction(ClustersState state) => state with
  // {
  //   IsLoading = true,
  //   Clusters = []
  // };
  // [ReducerMethod]
  // public static ClustersState ReduceFetchRoutesResultAction(ClustersState state, FetchClustersResultAction action) =>
  // state with
  // {
  //   IsLoading = false,
  //   Clusters = action.Routes
  // };

  [ReducerMethod(typeof(FetchGatewayAction))]
  public static GatewayState ReduceFetchDataAction(GatewayState state) => state with
  {
    IsLoading = true,
    Clusters = [],
    Routes = []
  };
  [ReducerMethod]
  public static GatewayState ReduceFetchRoutesResultAction(GatewayState state, FetchGatewayResultAction action) =>
  state with
  {
    IsLoading = false,
    Clusters = action.Clusters,
    Routes = action.Routes
  };
}