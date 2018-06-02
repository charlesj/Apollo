import React from 'react'
import moment from 'moment'
import apollo from '../../services/apolloServer'
import { Container, Button, } from '../_controls'

import './LoginSessions.css'

class LoginSessions extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      sessions: [],
    }

    this.formatDate = this.formatDate.bind(this)
    this.loadSessions = this.loadSessions.bind(this)
    this.revokeLoginSession = this.revokeLoginSession.bind(this)
  }

  componentDidMount() {
    this.loadSessions()
  }

  loadSessions() {
    this.setState(() => {
      return {
        entries: [],
      }
    })

    apollo.invoke('GetAllActiveLoginSessions', {}).then(data => {
      this.setState(() => {
        return {
          sessions: data,
        }
      })
    })
  }

  formatDate(date) {
    var converted = moment(date)
    return converted.format('Y-MM-DD HH:mm')
  }

  revokeLoginSession(token) {
    apollo
      .invoke('RevokeLoginSession', {
        tokenToRevoke: token,
      })
      .then(() => {
        this.loadSessions()
      })
  }

  render() {
    return (
      <Container>
        <table className="loginSessionsTable">
          <thead>
            <tr>
              <th>id</th>
              <th>token</th>
              <th>created</th>
              <th>last seen</th>
              <th>ip address</th>
              <th>user agent</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {this.state.sessions.map(function(session) {
              return (
                <tr key={session.id}>
                  <td>{session.id}</td>
                  <td>
                    <span title={session.token}>
                      {session.token.slice(0, 6)}...
                    </span>
                  </td>
                  <td>{this.formatDate(session.created_at)}</td>
                  <td>{this.formatDate(session.last_seen)}</td>
                  <td>{session.ip_address}</td>
                  <td>
                    <span title={session.user_agent}>
                      {session.user_agent && session.user_agent.slice(0, 20)}...
                    </span>
                  </td>
                  <td>
                    <Button
                      onClick={() => this.revokeLoginSession(session.token)}
                    >
                      revoke
                    </Button>
                  </td>
                </tr>
              )
            }, this)}
          </tbody>
        </table>
      </Container>
    )
  }
}

export default LoginSessions
