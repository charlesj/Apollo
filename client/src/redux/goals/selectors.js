import _ from "lodash";
import moment from "moment";
import * as metricsSelectors from "../metrics/selectors";

export function selectFeaturedGoal(state) {
  const { goals } = state.goals;
  if (goals.length === 0) {
    return null;
  }
  let goal;

  const featuredGoals = goals.filter(g => {
    return g.featured;
  });

  if (featuredGoals.length === 0) {
    goal = { ...goals[0] };
  } else {
    goal = { ...featuredGoals[0] };
  }

  const metrics = _.orderBy(
    metricsSelectors.byName(state, goal.metricName),
    "id",
    "asc"
  );

  const metricValues = metrics.map(m => m.value)
  if (metrics.length > 0) {
    const firstValue = _.head(metrics).value;
    const latestValue = _.last(metrics).value;
    const largestValue = _.max(metricValues)
    const lowestValue = _.min(metricValues)

    goal.metrics = metrics;
    goal.firstValue = firstValue;
    goal.latestValue = latestValue;

    if(goal.targetValue > firstValue){
      goal.totalDiff = _.round(goal.targetValue - lowestValue, 2);
      goal.actualDiff = _.round(goal.targetValue - goal.latestValue, 2);
      goal.completedDiff = _.round(latestValue - lowestValue, 2);
    }else {
      goal.totalDiff = _.round(largestValue - goal.targetValue, 2);
      goal.actualDiff = _.round(goal.latestValue -goal.targetValue, 2);
      goal.completedDiff = _.round(largestValue - latestValue, 2);
    }
    const percent = goal.completedDiff / goal.totalDiff * 100
    goal.percentComplete = _.round(percent,2)

    goal.graphData = metrics.map(m => {
      return {
        key: moment(m.created_at).format("Y-MM-DD"),
        val: m.value
      };
    });

    goal.graphMin = Math.min(goal.firstValue, goal.targetValue) - 10;
    goal.graphMax = Math.max(goal.firstValue, goal.targetValue) + 10;
  }

  return goal;
}
