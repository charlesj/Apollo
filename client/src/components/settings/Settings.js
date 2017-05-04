var React = require('react');
var apollo = require('../../services/apollo-server');

class Settings extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentPassword:'',
      newPassword: '',
      newPasswordVerification: '',
      showError: false,
      showSuccess: true
    }
  }

  render() {
    return (
      <div className='container-fluid'>
        <h2>Settings</h2>
      </div>
    )
  }
}

module.exports = Settings;
