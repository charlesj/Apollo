import _ from "lodash";
import moment from "moment";

const healthMetricsToRecord = [
  { type: "health", name: "weight", label: "Weight (lb)" },
  { type: "health", name: "systolic", label: "Systolic (upper bp)" },
  { type: "health", name: "diastolic", label: "Diastolic (lower bp)" },
  { type: "health", name: "temperature", label: "Temp (F)" },
  { type: "health", name: "heartrate", label: "Heartrate BPM" },
  { type: "health", name: "bloodOxygen", label: "Blood Oxygen %" },
  { type: "health", name: "sleepTime", label: "Sleep time (hours)" },
  { type: "health", name: "ketone", label: "Ketones (mmol/L)" },
  { type: "health", name: "bmi", label: "BMI" },
  { type: "health", name: "body_fat", label: "Body Fat %" },
  {
    type: "health",
    name: "fat_free_weight",
    label: "Fat-free body weight (lb)"
  },
  { type: "health", name: "body_water", label: "Body Water %" },
  { type: "health", name: "skeletal_muscle", label: "Skeletal Muscle %" },
  { type: "health", name: "muscle_mass", label: "Muscle Mass (lb)" },
  { type: "health", name: "bone_mass", label: "Bone Mass (lb)" },
  { type: "health", name: "protein", label: "Protein %" },
  { type: "health", name: "bmr", label: "BMR" },
  { type: "health", name: "metabolic_age", label: "Metabolic Age" }
];

export function healthMetrics(){
  return healthMetricsToRecord
}

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

export function healthCharts(state){
  const endDate = moment().startOf("day").add(1, 'days');
  const startDate = moment()
    .startOf("day")
    .subtract(3, "months");

  return healthMetricsToRecord.map(hm => {
    const chartData = dailyChartData(state, hm.name, moment(startDate), moment(endDate))
    const values = chartData.filter(m => m.value).map(m => m.value)
    const minValue = values.length > 0 ? _.min(values) : 0
    const maxValue = values.length > 0 ? _.max(values) : 100

    const range = maxValue - minValue
    const rangePadding = range * 0.01
    const min = _.floor(minValue - rangePadding)
    const max = _.ceil(maxValue + rangePadding)

    return {
      ...hm,
      min,
      max,
      chartData,
    }
  })
}
