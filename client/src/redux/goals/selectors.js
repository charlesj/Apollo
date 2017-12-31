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

  if (metrics.length > 0) {
    const firstMetric = _.head(metrics);
    const latestMetric = _.last(metrics);
    console.log({ metrics, firstMetric, latestMetric });
    goal.metrics = metrics;
    goal.firstValue = firstMetric.value;
    goal.latestValue = latestMetric.value;
    goal.targetDiff = Math.round(goal.targetValue - goal.firstValue, -2);
    goal.actualDiff = Math.round(goal.targetValue - goal.latestValue, -2);
    goal.completedDiff = Math.round(goal.firstValue - goal.latestValue, -2);
    if (goal.targetDiff > 0) {
      goal.percentComplete = Math.round(
        goal.actualDiff / goal.targetDiff * 100
      );
    } else {
      goal.percentComplete =
        100 - Math.round(goal.actualDiff / goal.targetDiff * 100);
    }

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
