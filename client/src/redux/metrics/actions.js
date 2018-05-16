import { dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";
import actions from "./actionCreators";
import { addMetric } from "../../services/metrics-service";
//import {NotifyError} from '../../services/notifier'

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

export function addMetrics(metricInfo) {
  return dispatchBasicActions(actions.addMetrics, async () => {
    const newMetrics = [];
    for (const metric of metricInfo) {
      const added = await addMetric(metric.category, metric.name, metric.value);
      newMetrics.push(added);
    }
    return newMetrics;
  });
}
