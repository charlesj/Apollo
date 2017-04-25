var React = require('react');
var PropTypes = require('prop-types');
var apollo = require('../services/apollo-server');

function DisplayServerInfo(props) {
  return (
    <div>
      <table>
        <tbody>
        <tr>
          <td>Version: </td>
          <td>{props.version}</td>
        </tr>
        <tr>
          <td>Commit Hash: </td>
          <td>{props.hash}</td>
        </tr>
        <tr>
          <td>Compiled On: </td>
          <td>{props.compiledOn}</td>
        </tr>
        </tbody>
      </table>
      <button onClick={props.hideInfo.bind(null, false)}>Hide</button>
    </div>
    );
}

DisplayServerInfo.propTypes = {
  version: PropTypes.string.isRequired,
  hash: PropTypes.string.isRequired,
  compiledOn: PropTypes.string.isRequired,
  hideInfo: PropTypes.func.isRequired
}

function HiddenServerInfo(props) {
  return (
    <div><button onClick={props.showInfo.bind(null, true)}>Show Server Info</button></div>
  )
}

HiddenServerInfo.PropTypes = {
  showInfo: PropTypes.func.isRequired
}

class ServerInfo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showInfo: false,
      serverInfo: null
    }

    this.getServerInfo = this.getServerInfo.bind(this);
    this.toggleInfo = this.toggleInfo.bind(this);
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

  toggleInfo(show) {
    this.setState(function() {
      return {
        showInfo: show
      }
    });
  }

  render() {
    if (this.state.serverInfo == null) {
      return <div>loading...</div>
    }

    return (
      <div>
        { this.state.showInfo
        ? <DisplayServerInfo
        version={this.state.serverInfo.version}
        hash={this.state.serverInfo.commitHash}
        compiledOn={this.state.serverInfo.compiledOn}
        hideInfo={this.toggleInfo} />
        : <HiddenServerInfo showInfo={this.toggleInfo} />
      }
      </div>
    )
  }
}

module.exports = ServerInfo;
