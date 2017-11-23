import React from 'react';
import { Button, Intent } from "@blueprintjs/core";
import metricsService from '../../services/metrics-service';
import { Notifier } from '../../services/notifier';

class Health extends React.Component {
  constructor(props) {
    super(props);
    this.state = this.getInitialState();

    this.updateWeight = this.updateWeight.bind(this);
    this.updateSystolic = this.updateSystolic.bind(this);
    this.updateDiastolic = this.updateDiastolic.bind(this);
    this.updateTemperature = this.updateTemperature.bind(this);
    this.updateHeartrate = this.updateHeartrate.bind(this);
    this.updateBloodOxygen = this.updateBloodOxygen.bind(this);
    this.getInitialState = this.getInitialState.bind(this);
    this.updateSleepTime = this.updateSleepTime.bind(this);
    this.save = this.save.bind(this);
  }

  getInitialState() {
    return {
      weight: '',
      systolic: '',
      diastolic: '',
      temperature: '',
      heartrate: '',
      bloodOxygen: '',
      sleepTime: ''
    };
  }

  updateWeight(event) {
    this.setState({
      weight: event.target.value
    });
  }

  updateSystolic(event) {
    this.setState({
      systolic: event.target.value
    });
  }

  updateDiastolic(event) {
    this.setState({
      diastolic: event.target.value
    });
  }

  updateTemperature(event) {
    this.setState({
      temperature: event.target.value
    });
  }

  updateHeartrate(event) {
    this.setState({
      heartrate: event.target.value
    });
  }

  updateBloodOxygen(event) {
    this.setState({
      bloodOxygen: event.target.value
    });
  }

  updateSleepTime(event) {
    this.setState({
      sleepTime: event.target.value
    });
  }

  save() {
    var requests = [
      metricsService.addMetric('health', 'weight', this.state.weight),
      metricsService.addMetric('health', 'systolic', this.state.systolic),
      metricsService.addMetric('health', 'diastolic', this.state.diastolic),
      metricsService.addMetric('health', 'temperature', this.state.temperature),
      metricsService.addMetric('health', 'heartrate', this.state.heartrate),
      metricsService.addMetric('health', 'bloodOxygen', this.state.bloodOxygen),
      metricsService.addMetric('health', 'sleepTime', this.state.sleepTime),
    ];

    Promise.all(requests).then(values => {
      this.setState({
        weight: '',
        systolic: '',
        diastolic: '',
        temperature: '',
        heartrate: '',
        bloodOxygen: '',
        sleepTime: ''
      })
      Notifier.show({
        intent: Intent.SUCCESS,
        message: "Health information recorded successfully",
      })
    }).catch(reason => {
      console.log('at least one metric failes');
      console.log(reason);
      Notifier.show({
        intent: Intent.DANGER,
        message: "Error recording at least some of this information",
      })
    });
  }

  render() {
    return (<div>
                {this.state.showSuccess && <p className="pt-callout pt-intent-success">Successfully submitted healthsheet</p>}
                {this.state.showError && <p className="pt-callout pt-intent-danger">Something went wrong</p>}
                <div className="grid-form pt-card">
                    <fieldset>
                        <legend>Daily Health Sheet</legend>
                        <div data-row-span="4">
                            <div data-field-span="1">
                                <label>Weight (lb)  </label>
                                <input type="text" onChange={this.updateWeight} value={this.state.weight} />
                            </div>
                            <div data-field-span="1">
                                <label>Blood Pressure Systolic (top)</label>
                                <input type="text" onChange={this.updateSystolic} value={this.state.systolic} />
                            </div>
                            <div data-field-span="1">
                                <label>Blood Pressure Diastolic (bottom)</label>
                                <input type="text" onChange={this.updateDiastolic} value={this.state.diastolic} />
                            </div>
                            <div data-field-span="1">
                                <label>Temperature (F)</label>
                                <input type="text" onChange={this.updateTemperature} value={this.state.temperature} />
                            </div>
                        </div>
                        <div data-row-span="4">
                            <div data-field-span="1">
                                <label>Heart Rate (bpm)</label>
                                <input type="text" onChange={this.updateHeartrate} value={this.state.heartrate} />
                            </div>
                            <div data-field-span="1">
                                <label>Blood Oxygen</label>
                                <input type="text" onChange={this.updateBloodOxygen} value={this.state.bloodOxygen} />
                            </div>
                            <div data-field-span="2">
                                <label>Sleep Time (hours)</label>
                                <input type="text" onChange={this.updateSleepTime} value={this.state.sleepTime} />
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div className='buttonSpace'>
                    <Button
      className='pt-button pt-intent-primary'
      iconName="add"
      text="Add Health Metrics"
      onClick={this.save}
      /></div>
                </div>)
  }
}

module.exports = Health;
