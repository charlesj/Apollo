import React from "react";
import { connect } from "react-redux";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
} from "recharts";
import { metricsActions } from "../../redux/actions";
import { metricsSelectors } from "../../redux/selectors";
import { FlexRow } from "../_controls";

import './homehealth.css'

class HomeHealth extends React.Component {
  componentDidMount() {
    this.props.loadData();
  }

  render() {
    const { charts } = this.props;

    return (
      <FlexRow wrap>
        { charts.map(chart => {
          return (<div className='chart'>
            <div className='chartTitle'>{chart.label}</div>
            <LineChart width={600} height={300} data={chart.chartData}>
            <XAxis dataKey="date" />
            <YAxis type="number" domain={[chart.min, chart.max]} />
            <CartesianGrid strokeDasharray="3 3" />
            <Tooltip />
            <Line type="monotone" dataKey="value" stroke="#1F4B99"/>
          </LineChart>
        </div>)
        })}
      </FlexRow>
    );
  }
}

function mapStateToProps(state) {
  return {
    charts: metricsSelectors.healthCharts(state)
  };
}

function mapDispatchToProps(dispatch) {
  return {
    loadData: () => dispatch(metricsActions.loadMetrics("health"))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(HomeHealth);
