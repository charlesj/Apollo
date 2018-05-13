import React from "react";
import _ from "lodash";
import { connect } from "react-redux";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend
} from "recharts";
import { metricsActions } from "../../redux/actions";
import { metricsSelectors } from "../../redux/selectors";

class HomeHealth extends React.Component {
  componentDidMount() {
    this.props.loadData();
  }

  render() {
    const { weightData } = this.props;
    const values = weightData.filter(m => m.value).map(m => m.value);
    const weightMin = values.length > 0 ? _.min(values) - 10 : 0;
    const weightMax = values.length > 0 ? _.max(values) + 10 : 350;

    return (
      <div>
        <LineChart width={600} height={300} data={weightData}>
          <XAxis dataKey="date" />
          <YAxis type="number" domain={[weightMin, weightMax]} />
          <CartesianGrid strokeDasharray="3 3" />
          <Tooltip />
          <Legend verticalAlign="bottom" height={36} />
          <Line type="monotone" dataKey="value" stroke="#1F4B99" />
        </LineChart>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    weightData: metricsSelectors.weightChartData(state)
  };
}

function mapDispatchToProps(dispatch) {
  return {
    loadData: () => dispatch(metricsActions.loadMetrics("health"))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(HomeHealth);
