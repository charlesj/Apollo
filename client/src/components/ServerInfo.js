var React = require('react');
var apollo = require('../services/apollo-server');
var moment = require('moment');

class ServerInfo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showInfo: false,
      serverInfo: null
    }

    this.getServerInfo = this.getServerInfo.bind(this);
  }

  componentDidMount() {
    this.getServerInfo();
  }

  getServerInfo() {
    this.setState(function() {
      return {
        serverInfo: null
      };
    });

    apollo.invoke('applicationInfo', {})
      .then(function(data) {
        this.setState(function() {
          return {
            serverInfo: data
          };
        });
      }.bind(this));
  }

  render() {
    if (this.state.serverInfo == null) {
      return <div>loading...</div>
    }
    var version = this.state.serverInfo.version;
    var shortHash = this.state.serverInfo.commitHash.slice(0, 6);
    var compiledOn = moment(this.state.serverInfo.compiledOn);
    return (
      <div>{ compiledOn.format('YYYY-MM-DD HH:mm')}  v{version} ({shortHash})
      </div>
    )
  }
}

module.exports = ServerInfo;
