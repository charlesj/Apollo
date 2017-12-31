import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";

const actionCreators = createActions({
  metrics: {
    loadMetrics: basicActions()
  }
});

const actions = actionCreators.metrics;

export default actions;

export function loadMetrics(category, name) {
  return dispatchBasicActions(actions.loadMetrics, async () => {
    const metrics = await apolloServer.invoke("getMetrics", { name, category });
    if (Array.isArray(metrics)) {
      return { metrics };
    } else {
      return { metrics: [] };
    }
  });
}
