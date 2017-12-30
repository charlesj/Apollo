import React from "react";
import moment from "moment";
import apollo from "../../services/apolloServer";

class LoginSessions extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      sessions: []
    };

    this.formatDate = this.formatDate.bind(this);
    this.loadSessions = this.loadSessions.bind(this);
    this.revokeLoginSession = this.revokeLoginSession.bind(this);
  }

  componentDidMount() {
    this.loadSessions();
  }

  loadSessions() {
    this.setState(() => {
      return {
        entries: []
      };
    });

    apollo.invoke("GetAllActiveLoginSessions", {}).then(data => {
      this.setState(() => {
        return {
          sessions: data
        };
      });
    });
  }

  formatDate(date) {
    var converted = moment(date);
    return converted.format("Y-MM-DD HH:mm");
  }

  revokeLoginSession(token) {
    apollo
      .invoke("RevokeLoginSession", {
        tokenToRevoke: token
      })
      .then(() => {
        this.loadSessions();
      });
  }

  render() {
    return (
      <div>
        <table className="pt-table pt-bordered">
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
            {this.state.sessions.map(function(session, index) {
              return (
                <tr key={session.id}>
                  <td>{session.id}</td>
                  <td>{session.token}</td>
                  <td>{this.formatDate(session.created_at)}</td>
                  <td>{this.formatDate(session.last_seen)}</td>
                  <td>{session.ip_address}</td>
                  <td>{session.user_agent}</td>
                  <td>
                    <button
                      onClick={this.revokeLoginSession.bind(
                        null,
                        session.token
                      )}
                    >
                      revoke
                    </button>
                  </td>
                </tr>
              );
            }, this)}
          </tbody>
        </table>
      </div>
    );
  }
}

export default LoginSessions;
