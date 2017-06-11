var React = require('react');
import { Button } from "@blueprintjs/core";

class Health extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      sleepQuality: 1
    }
  }
  render() {
    return (<div>

<div className="grid-form">
	<fieldset>
		<legend>Daily Health Sheet Quantitative</legend>
		<div data-row-span="4">
			<div data-field-span="1">
				<label>Weight (lb)  </label>
				<input type="text" />
			</div>
			<div data-field-span="1">
				<label>Blood Pressure Systolic (top)</label>
				<input type="text" />
			</div>
            <div data-field-span="1">
				<label>Blood Pressure Diastolic (bottom)</label>
				<input type="text" />
			</div>
            <div data-field-span="1">
				<label>Temperature (F)</label>
				<input type="text" />
			</div>
		</div>
    <div data-row-span="4">
        <div data-field-span="1">
            <label>Heart Rate (bpm)</label>
            <input type="text" />
        </div>
        <div data-field-span="1">
            <label>Blood Oxygen</label>
            <input type="text" />
        </div>
        <div data-field-span="2">
            <label>Sleep Time (hours)</label>
            <input type="text" />
        </div>
    </div>
	</fieldset>
</div>
<div className='healthFormButtons'>
<Button
      className='pt-button pt-intent-success'
      iconName="add"
      text="Add Health Metrics"
      /></div>
        </div>)
  }
}

module.exports = Health;
