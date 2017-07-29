import React from 'react';
import apollo from '../services/apollo-server';
import moment from 'moment';

class ServerInfo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showInfo: false,
      serverInfo: null
    }

    this.getServerInfo = this.getServerInfo.bind(this);
    setInterval(this.getServerInfo, 900000); // every 15 minutes
  }

  componentDidMount() {
    this.getServerInfo();
  }

  getServerInfo() {
    apollo.invoke('applicationInfo', {})
      .then((data) => {
        if(this.state.serverInfo != null && !this.state.updateAvailable && data.version !== this.state.serverInfo.version){
          this.setState({updateAvailable: true});
        }
        if(this.state.updateAvailable){
            return;
        }

        this.setState({ serverInfo: data });
      })
      .catch(() => {
        console.log("error communicating to server");
      });
  }


  render() {
    if (this.state.serverInfo == null) {
      return <div>loading...</div>
    }
    var version = this.state.serverInfo.version;
    var shortHash = this.state.serverInfo.commitHash.slice(0, 6);
    var compiledOn = moment(this.state.serverInfo.compiledOn);
    return (
      <span> { this.state.updateAvailable && (<i className="updateAvailable">Update Available!</i>)}  Apollo v{version} ({shortHash}) compiled on { compiledOn.format('YYYY-MM-DD HH:mm')} </span>
    )
  }
}

module.exports = ServerInfo;
