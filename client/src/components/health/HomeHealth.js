var React = require('react');
var _ = require('lodash');
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip } from 'recharts';

var metricsService = require('../../services/metrics-service');

class HomeHealth extends React.Component {

  constructor(props){
      super(props);
      this.state = {
          weightData: [],
          bpData: [],
          sleepTimeData: []
      };

      this.extractBloodPressure = this.extractBloodPressure.bind(this);
  }

  componentDidMount() {
    this.loadData();
  }

  loadData(){
    metricsService.getMetrics('health')
        .then(metrics => {
          var weightMetrics = metrics
                    .filter(m => {return m.name === "weight";})
                    .map(m => { return {key: m.created_at, weight: m.value};});

          var bloodPressureMetrics = this.extractBloodPressure(metrics);

          var sleepTime = metrics
                    .filter(m => {return m.name === "sleepTime";})
                    .map(m => { return {key: m.created_at, sleepTime: m.value};});

          this.setState({
              weightData: weightMetrics,
              bpData: bloodPressureMetrics,
              sleepTimeData: sleepTime
          });
        });
  }

  extractBloodPressure(metrics){
    var diastolic = metrics
                    .filter(m => {return m.name === "diastolic";})
                    .map(m => {return m.value});
    var systolic = metrics
                .filter(m => {return m.name === "systolic";})
                .map(m => {return m.value});
    var temperature = metrics
                .filter(m => {return m.name === "temperature";})
                .map(m => {return m.value});
    var heartRate = metrics
            .filter(m => {return m.name === "heartrate";})
            .map(m => {return m.value});
    var bloodOxygen = metrics
            .filter(m => {return m.name === "bloodOxygen";})
            .map(m => {return m.value});
    var combined = _.zip(systolic, diastolic, temperature, heartRate, bloodOxygen);
    var counter = 0;
    return combined.map(c => {
        return {
            key: counter++,
            systolic: c[0],
            diastolic: c[1],
            temperature: c[2],
            heartrate: c[3],
            bloodOxygen: c[4]
        }
    });
  }

  render() {
    return (<div>
        <div className='chartContainer'>
        <LineChart width={300} height={150} data={this.state.weightData}>
        <XAxis dataKey="key"/>
               <YAxis/>
               <CartesianGrid strokeDasharray="3 3"/>
               <Tooltip/>
               <Line type="monotone" dataKey="weight" stroke="#8884d8" activeDot={{r: 8}}/>
        </LineChart>
        </div>
        <div className='chartContainer'>
        <LineChart width={300} height={150} data={this.state.bpData}>
        <XAxis dataKey="key"/>
               <YAxis/>
               <CartesianGrid strokeDasharray="3 3"/>
               <Tooltip/>
               <Line type="monotone" dataKey="diastolic" stroke="#0E5A8A" />
               <Line type="monotone" dataKey="systolic" stroke="#0A6640" />
               <Line type="monotone" dataKey="temperature" stroke="#A66321" />
               <Line type="monotone" dataKey="heartrate" stroke="#A82A2A" />
               <Line type="monotone" dataKey="bloodOxygen" stroke="#1F4B99" />
        </LineChart>
        </div>
        <div className='chartContainer'>
        <LineChart width={300} height={150} data={this.state.sleepTimeData}>
        <XAxis dataKey="key"/>
               <YAxis/>
               <CartesianGrid strokeDasharray="3 3"/>
               <Tooltip/>
               <Line type="monotone" dataKey="sleepTime" stroke="#8884d8" activeDot={{r: 8}}/>
        </LineChart>
        </div>
    </div>);
  }
}

module.exports = HomeHealth;
