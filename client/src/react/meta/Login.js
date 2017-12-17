import React, { Component } from "react";
import { connect } from "react-redux";
import { metaActions } from "../../redux/actions";

import "./Login.css";

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = {
      password: "",
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
    const { login } = this.props;
    login(this.state.password);
    this.setState({
      password: ""
    });
  }

  render() {
    const { loginError } = this.props;
    return (
      <div className="loginView">
        <form onSubmit={this.handleSubmit}>
          <input
            type="password"
            id="password"
            className="pt-input pt-intent-success"
            value={this.state.password}
            onChange={this.handleChange}
          />
          {loginError && <div className="loginError">Wrong passsword</div>}
        </form>
      </div>
    );
  }
}

function mapStateToProps(state, props) {
  const { loginError } = state.meta;

  return {
    loginError
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    login: password => dispatch(metaActions.login(password))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Login);
