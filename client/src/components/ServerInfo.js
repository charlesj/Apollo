var React = require('react');
var apollo = require('../services/apollo-server');

class ServerInfo extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            serverInfo: null
        }

        this.getServerInfo = this.getServerInfo.bind(this);
    }

    componentDidMount(){
        this.getServerInfo();
    }

    getServerInfo() {
        this.setState(function(){
            return { serverInfo: null };
        });

        apollo.invoke('applicationInfo', {})
            .then(function(data){
                this.setState(function(){
                    return { serverInfo: data };
                });
            }.bind(this));
    }

    render() {
        if(this.state.serverInfo == null){
            return <div>loading...</div>
        }

        return (
            <div>
                version: {this.state.serverInfo.version} <br />
                hash: {this.state.serverInfo.commitHash} <br />
                compiled: {this.state.serverInfo.compiledOn}
            </div>
       )
    }
}

module.exports = ServerInfo;
