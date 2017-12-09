import React from 'react';
import ReactJson from 'react-json-view'
import apolloServer from '../../services/apollo-server';


function MemberDisplay(props){
  return (<div className="commandMember">
    <label className="pt-label">
      {props.member.name}
      <input className="pt-input" type="text" />
    </label>
    {props.member.members && props.member.members.map(m => {
      return (<MemberDisplay member={m} key={m.name} />)
    })}
  </div>)
}

function CommandInfo(props){
  return (<div className="commandDescription">
    <div className="commandDescriptionHeader">Command: {props.command}</div>
    <div className="commandPayloadContainer">
      <textarea value={props.payload} onChange={props.updatePayload} rows="13" cols="100" />
    </div>
    <button className="pt-button pt-intent-danger" onClick={props.execute.bind(null, props.command, props.payload)}>Execute</button>
  </div>);
}

function getExecutionBorderStyle(resultType){
  var styles = {
    "Success": "#0A6640",
    "Error": "#A82A2A",
    "Invalid": "#A66321"
  };
  return {
    "border": "20px solid " + styles[resultType]
  };
}

function CommandResult(props){
  return (<div className="commandExecutionResult">
    Result: {props.lastExecution.result.ResultStatus} ({props.lastExecution.id}: {props.lastExecution.result.Elapsed}ms)
    { props.lastExecution.error && <div className="commandExecutionError">{props.lastExecution.error}: {props.lastExecution.result.ErrorMessage}</div>}
    <div className="commandResultJsonContainer" style={getExecutionBorderStyle(props.lastExecution.result.ResultStatus)}>
      <ReactJson src={props.lastExecution.result.Result} theme="solarized" name={null} />
    </div>
  </div>)
}

class CommandInterface extends React.Component{
  constructor(props){
    super(props);

    this.state = {
      commands: [],
      selectedCommand: null,
      lastExecution: null,
      payload: null
    };

    this.execute = this.execute.bind(this);
    this.selectCommand = this.selectCommand.bind(this);
    this.updatePayload = this.updatePayload.bind(this);
  }

  componentDidMount(){
    apolloServer.invoke('GetAvailableCommands', {}).then(commands => {
      this.setState({commands});
    })
  }

  selectCommand(command){
    apolloServer.invoke("GetExamplePayload", {command}).then(payload => {
      this.setState({selectedCommand: command, payload: JSON.stringify(payload, null, 2)});
    });
  }

  updatePayload(e){
    this.setState({payload: e.target.value});
  }

  execute(command, parameters){
    apolloServer.invokeFull(command, JSON.parse(parameters)).then(result => {
      this.setState({ lastExecution: result});
    })
  }

  render(){
    return (<div className="commandInterfaceContainer">
      <div className="commandListSelector">
        {this.state.commands.map(cmd => {
          return <div onClick={this.selectCommand.bind(null, cmd)} key={cmd}>{cmd}</div>;
        })}
      </div>
      <div className="commandExplorer">
      {this.state.selectedCommand && <CommandInfo
              payload={this.state.payload}
              command={this.state.selectedCommand}
              execute={this.execute}
              updatePayload={this.updatePayload}
            />}
      {this.state.lastExecution && <CommandResult lastExecution={this.state.lastExecution} />}
      </div>
    </div>);
  }
}

module.exports = CommandInterface;
