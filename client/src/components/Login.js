import React from 'react';
import apollo from '../services/apollo-server';

class Login extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      password: '',
      showWrong: false,
      attempts: 0
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    this.setState({
      password: event.target.value
    });
  }

  handleSubmit(event) {
    event.preventDefault();

    apollo.invoke("Login", {
      password: this.state.password
    })
      .then((data) => {
        if (data.token != null) {
          this.setState({
            password: ''
          });
          this.props.onLogin(data.token);
        } else {
          this.setState({
            showWrong: true,
            attempts: this.state.attempts + 1
          });
        }
      }).catch(err => {
      this.setState({
        showWrong: true,
        attempts: this.state.attempts + 1
      });
    });
  }

  render() {
    return (<div id="login" className="pt-non-ideal-state">
        <form onSubmit={this.handleSubmit}>
          <div className="pt-non-ideal-state-description"><div className="pt-input-group .modifier">
              <span className="pt-icon pt-icon-log-in"></span>

              <input
      type='password'
      id='password'
      className='pt-input pt-intent-success'
      value={this.state.password}
      onChange={this.handleChange}/>
      </div>
          </div>
        </form>
        {this.state.showWrong && <div>Wrong password ({this.state.attempts} attempts)</div>}
    </div>)
  }
}

module.exports = Login
