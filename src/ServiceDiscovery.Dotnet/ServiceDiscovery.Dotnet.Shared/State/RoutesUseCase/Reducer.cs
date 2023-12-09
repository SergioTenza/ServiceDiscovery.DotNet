using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;

public static class Reducer
{
  [ReducerMethod(typeof(FetchRoutesAction))]
  public static RoutesState ReduceFetchDataAction(RoutesState state) => state with
  {
    IsLoading = true,
    Routes = []
  };
  [ReducerMethod]
  public static RoutesState ReduceFetchRoutesResultAction(RoutesState state, FetchRoutesResultAction action) =>
  state with
  {
    IsLoading = false,
    Routes = action.Routes
  };
}