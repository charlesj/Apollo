import React from "react";
import { addMetric } from "../../services/metrics-service";
import { NotifySuccess, NotifyError } from "../../services/notifier";

const metricsToRecord = [
  { type: 'health', name: 'weight', label: 'Weight (lb)' },
  { type: 'health', name: 'systolic', label: 'Systolic (upper bp)' },
  { type: 'health', name: 'diastolic', label: 'Diastolic (lower bp)' },
  { type: 'health', name: 'temperature', label: 'Temp (F)' },
  { type: 'health', name: 'heartrate', label: 'Heartrate BPM' },
  { type: 'health', name: 'bloodOxygen', label: 'Blood Oxygen Percent' },
  { type: 'health', name: 'sleepTime', label: 'Sleep time (hours)' },
  { type: 'health', name: 'ketone', label: 'Ketones (mmol/L)' },
  { type: 'health', name: 'bmi', label: 'BMI' },
  { type: 'health', name: 'body_fat', label: 'Body Fat %' },
  { type: 'health', name: 'fat_free_weight', label: 'Fat-free body weight (lb)' },
  { type: 'health', name: 'body_water', label: 'Body Water' },
  { type: 'health', name: 'skeletal_muscle', label: 'Skeletal Muscle &' },
  { type: 'health', name: 'muscle_mass', label: 'Muscle Mass (lb)' },
  { type: 'health', name: 'bone_mass', label: 'Bone Mass (lb)' },
  { type: 'health', name: 'protein', label: 'Protein %' },
  { type: 'health', name: 'bmr', label: 'BMR' },
  { type: 'health', name: 'metabolic_age', label: 'Metabolic Age' },
]

class Health extends React.Component {
  constructor(props) {
    super(props);
    this.state = this.buildInitialState()
  }

  buildInitialState() {
    return metricsToRecord.reduce((acc, i) => {
      acc[i.name] = ''
      return acc
    }, {})
  }

  handleChange(name, value){
    this.setState({[name]: value})
  }

  save() {
    const metricsWithValues = metricsToRecord.filter(m => this.state[m.name] !== '').map(m => {
      return {name: m.name, value: this.state[m.name]}
    })

    const requests = metricsWithValues.map(m => addMetric('health', m.name, m.value))

    Promise.all(requests)
      .then(values => {
        this.setState(this.buildInitialState());
        NotifySuccess("Health information recorded successfully");
      })
      .catch(reason => {
        console.log("at least one metric failes");
        console.log(reason);
        NotifyError("Error recording at least some of this information");
      });
  }

  render() {
    return (
      <div>
        <div>
          <fieldset>
            <legend>Daily Health Sheet</legend>
              { metricsToRecord.map(m => {
                return (<div key={m.name}>
                  <label>{m.label}</label>
                  <input
                    type="text"
                    onChange={(e) => this.handleChange(m.name, e.target.value)}
                    value={this.state[m.name]}
                  />
                </div>)
              })}
          </fieldset>
        </div>
        <div className="buttonSpace">
          <button onClick={() => this.save()}>Add Health Metrics</button>
        </div>
      </div>
    );
  }
}

export default Health;
