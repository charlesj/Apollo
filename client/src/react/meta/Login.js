import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { metaActions, } from '../../redux/actions'

import './Login.css'

class Login extends Component {
  constructor(props) {
    super(props)

    this.state = {
      password: '',
      showWrong: false,
      attempts: 0,
    }

    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
  }

  handleChange(event) {
    this.setState({
      password: event.target.value,
    })
  }

  handleSubmit(event) {
    event.preventDefault()
    const { login, } = this.props
    login(this.state.password)
    this.setState({
      password: '',
    })
  }

  render() {
    const { loginError, } = this.props
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
    )
  }
}

Login.propTypes = {
  login: PropTypes.func.isRequired,
  loginError: PropTypes.any,
}

Login.defaultProps = {
  loginError: null,
}

function mapStateToProps(state) {
  const { loginError, } = state.meta

  return {
    loginError,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    login: password => dispatch(metaActions.login(password)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Login)
