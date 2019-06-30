import React from 'react'
import apollo from '../../services/apolloServer'
import { Container, Button, } from '../_controls'
import { NotifySuccess, NotifyError, } from '../../services/notifier'

import './ChangePassword.css'

class ChangePassword extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      currentPassword: '',
      newPassword: '',
      newPasswordVerification: '',
    }
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value,
    })
  }

  async handleSubmit(event) {
    event.preventDefault()

    try {
      await apollo.invoke('ChangePassword', {
        currentPassword: this.state.currentPassword,
        newPassword: this.state.newPassword,
        newPasswordVerification: this.state.newPasswordVerification,
      })

      NotifySuccess('Changed Password')
      this.setState({
        currentPassword: '',
        newPassword: '',
        newPasswordVerification: '',
      })
    } catch (err) {
      NotifyError('Error changing password')
      this.setState({
        currentPassword: '',
        newPassword: '',
        newPasswordVerification: '',
      })
    }
  }

  render() {
    const {
      currentPassword,
      newPassword,
      newPasswordVerification,
    } = this.state
    return (
      <Container>
        <form
          className="changePasswordForm"
          onSubmit={e => this.handleSubmit(e)}
        >
          <input
            type="password"
            name="currentPassword"
            placeholder="Current Password"
            value={currentPassword}
            onChange={e => this.handleChange(e)}
          />
          <input
            type="password"
            name="newPassword"
            placeholder="New Password"
            value={newPassword}
            onChange={e => this.handleChange(e)}
          />
          <input
            type="password"
            name="newPasswordVerification"
            placeholder="Verify New Password"
            value={newPasswordVerification}
            onChange={e => this.handleChange(e)}
          />
          <Button type="submit">Change Password</Button>
        </form>
      </Container>
    )
  }
}

export default ChangePassword
