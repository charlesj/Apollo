var React = require('react');
var apollo = require('../../services/apollo-server');

class Settings extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentPassword: '',
      newPassword: '',
      newPasswordVerification: '',
      showError: false,
      showSuccess: false
    }

    this.updateCurrentPassword = this.updateCurrentPassword.bind(this);
    this.updateNewPassword = this.updateNewPassword.bind(this);
    this.updateNewPasswordVerification = this.updateNewPasswordVerification.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  updateCurrentPassword(event) {
    this.setState({
      currentPassword: event.target.value
    });
  }

  updateNewPassword(event) {
    this.setState({
      newPassword: event.target.value
    });
  }

  updateNewPasswordVerification(event) {
    this.setState({
      newPasswordVerification: event.target.value
    });
  }

  handleSubmit(event) {
    event.preventDefault();

    apollo.invoke('ChangePassword', {
      currentPassword: this.state.currentPassword,
      newPassword: this.state.newPassword,
      newPasswordVerification: this.state.newPasswordVerification
    }).then(data => {
      this.setState({
        showSuccess: true,
        showError: false,
        currentPassword: '',
        newPassword: '',
        newPasswordVerification: ''
      });
    }).catch(err => {
      console.log('Error changing password', err);
      this.setState({
        showSuccess: false,
        showError: true,
        currentPassword: '',
        newPassword: '',
        newPasswordVerification: ''
      });
    });
  }

  render() {
    return (
      <div className='container-fluid'>
        <h2>Settings</h2>
        <div className="col-xs-6 col-md-4">
        {this.state.showSuccess && <p className="bg-success">Successfully changed password</p>}
        <form onSubmit={this.handleSubmit}>
          <div className="pt-form-group">
            <label htmlFor="currentPassword" className="pt-label">Current Password</label>
            <input
      type="password"
      className="pt-input"
      id="currentPassword"
      placeholder="Current Password"
      value={this.state.currentPassword}
      onChange={this.updateCurrentPassword} />
          </div>
          <div className="pt-form-group">
            <label htmlFor="newPassword" className="pt-label">New Password</label>
            <input
      type="password"
      className="pt-input"
      id="newPassword"
      placeholder="New Password"
      value={this.state.newPassword}
      onChange={this.updateNewPassword} />
          </div>
          <div className="pt-form-group">
            <label htmlFor="newPasswordVerification" className="pt-label">Verify New Password</label>
            <input
      type="password"
      className="pt-input"
      id="newPasswordVerification"
      placeholder="Verify New Password"
      value={this.state.newPasswordVerification}
      onChange={this.updateNewPasswordVerification} />
          </div>
          <button type="submit" className="pt-button pt-intent-primary pt-icon-confirm">Change Password</button>
          {this.state.showError && <p className="bg-danger">Could not change password</p>}
        </form>
        </div>
      </div>
    )
  }
}

module.exports = Settings;
