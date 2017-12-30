import React from "react";
import _ from "lodash";
import moment from "moment";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend
} from "recharts";
import { getMetrics } from "../../services/metrics-service";
import FontAwesome from "react-fontawesome";

class HomeHealth extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      weightData: [],
      bpData: [],
      sleepTimeData: []
    };

    this.extractBloodPressure = this.extractBloodPressure.bind(this);
    this.formatDateTime = this.formatDateTime.bind(this);
    this.loadData = this.loadData.bind(this);
  }

  componentDidMount() {
    this.loadData();
  }

  loadData() {
    getMetrics("health").then(metrics => {
      var weightMetrics = metrics
        .filter(m => {
          return m.name === "weight";
        })
        .map(m => {
          return {
            key: this.formatDateTime(m.created_at),
            weight: m.value
          };
        });

      var bloodPressureMetrics = this.extractBloodPressure(metrics);

      var sleepTime = metrics
        .filter(m => {
          return m.name === "sleepTime";
        })
        .map(m => {
          return {
            key: this.formatDateTime(m.created_at),
            sleepTime: m.value
          };
        });

      this.setState({
        weightData: weightMetrics,
        bpData: bloodPressureMetrics,
        sleepTimeData: sleepTime
      });
    });
  }

  formatDateTime(datetime) {
    var converted = moment(datetime);
    return converted.format("Y-MM-DD");
  }

  extractBloodPressure(metrics) {
    var keys = metrics
      .filter(m => {
        return m.name === "diastolic";
      })
      .map(m => {
        return this.formatDateTime(m.created_at);
      });
    var diastolic = metrics
      .filter(m => {
        return m.name === "diastolic";
      })
      .map(m => {
        return m.value;
      });
    var systolic = metrics
      .filter(m => {
        return m.name === "systolic";
      })
      .map(m => {
        return m.value;
      });
    var temperature = metrics
      .filter(m => {
        return m.name === "temperature";
      })
      .map(m => {
        return m.value;
      });
    var heartRate = metrics
      .filter(m => {
        return m.name === "heartrate";
      })
      .map(m => {
        return m.value;
      });
    var bloodOxygen = metrics
      .filter(m => {
        return m.name === "bloodOxygen";
      })
      .map(m => {
        return m.value;
      });
    var combined = _.zip(
      systolic,
      diastolic,
      temperature,
      heartRate,
      bloodOxygen,
      keys
    );
    return combined.map(c => {
      return {
        key: c[5],
        systolic: c[0],
        diastolic: c[1],
        temperature: c[2],
        heartrate: c[3],
        bloodOxygen: c[4]
      };
    });
  }

  render() {
    return (
      <div className="healthCharts">
        <div>
          History{" "}
          <button
            className="textButton pt-intent-success"
            onClick={this.loadData}
          >
            <FontAwesome name="refresh" />
          </button>
        </div>
        <div className="chartsContainer">
          <div className="chartContainer pt-card">
            <LineChart width={600} height={300} data={this.state.weightData}>
              <XAxis dataKey="key" />
              <YAxis type="number" domain={[270, 315]} />
              <CartesianGrid strokeDasharray="3 3" />
              <Tooltip />
              <Legend verticalAlign="bottom" height={36} />
              <Line type="monotone" dataKey="weight" stroke="#1F4B99" />
            </LineChart>
          </div>
          <div className="chartContainer pt-card">
            <LineChart width={600} height={300} data={this.state.bpData}>
              <XAxis dataKey="key" />
              <YAxis type="number" domain={[40, 160]} />
              <CartesianGrid strokeDasharray="3 3" />
              <Tooltip />
              <Legend verticalAlign="bottom" height={36} />
              <Line type="monotone" dataKey="systolic" stroke="#0A6640" />
              <Line type="monotone" dataKey="diastolic" stroke="#0E5A8A" />
              <Line type="monotone" dataKey="temperature" stroke="#A66321" />
              <Line type="monotone" dataKey="heartrate" stroke="#A82A2A" />
              <Line type="monotone" dataKey="bloodOxygen" stroke="#1F4B99" />
            </LineChart>
          </div>
          <div className="chartContainer pt-card">
            <LineChart width={600} height={300} data={this.state.sleepTimeData}>
              <XAxis dataKey="key" />
              <YAxis />
              <CartesianGrid strokeDasharray="3 3" />
              <Tooltip />
              <Legend verticalAlign="bottom" height={36} />
              <Line type="monotone" dataKey="sleepTime" stroke="#1F4B99" />
            </LineChart>
          </div>
        </div>
      </div>
    );
  }
}

export default HomeHealth;
