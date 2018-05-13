import _ from "lodash";
import moment from "moment";

export function byName(state, name) {
  return state.metrics.metrics.filter(m => m.name === name);
}

export function dailyChartData(state, name, startDate, endDate) {
  const metrics = state.metrics.metrics.filter(
    m => m.name === name && m.created_day.isSameOrBefore(endDate)
  );
  let currDate = startDate;

  const data = [];
  while (currDate.isBefore(endDate)) {
    const dayMetrics = metrics
      .filter(m => m.created_day.isSame(currDate))
      .map(m => m.value);
    if (dayMetrics.length > 0) {
      data.push({
        date: currDate.format("YYYY-MM-DD"),
        value: _.round(_.mean(dayMetrics), 2)
      });
    } else {
      data.push({ date: currDate.format("YYYY-MM-DD"), value: null });
    }
    currDate.add(1, "days");
  }

  return data;
}

export function weightChartData(state) {
  const endDate = moment().startOf("day");
  const startDate = moment()
    .startOf("day")
    .subtract(3, "months");

  return dailyChartData(state, "weight", startDate, endDate);
}
