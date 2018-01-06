import React, { Component } from "react";
import ReactJson from "react-json-view";
import apolloServer from "../../services/apolloServer";
import { Button, Container, FlexRow, SelectList } from "../_controls";

import "./Commands.css";

function CommandInfo(props) {
  return (
    <div className="commandDescription">
      <div className="commandDescriptionHeader">Command: {props.command}</div>
      <div>
        <textarea
          value={props.payload}
          onChange={props.updatePayload}
          rows="13"
          cols="100"
        />
      </div>
      <Button
        primary
        onClick={props.execute.bind(null, props.command, props.payload)}
      >
        Execute
      </Button>
    </div>
  );
}

function getExecutionBorderStyle(resultType) {
  var styles = {
    Success: "#0A6640",
    Error: "#A82A2A",
    Invalid: "#A66321"
  };
  return {
    border: "20px solid " + styles[resultType]
  };
}

function CommandResult(props) {
  return (
    <div className="commandExecutionResult">
      Result: {props.lastExecution.result.ResultStatus} ({
        props.lastExecution.id
      }: {props.lastExecution.result.Elapsed}ms)
      {props.lastExecution.error && (
        <div className="commandExecutionError">
          {props.lastExecution.error}: {props.lastExecution.result.ErrorMessage}
        </div>
      )}
      <div
        className="commandResultJsonContainer"
        style={getExecutionBorderStyle(props.lastExecution.result.ResultStatus)}
      >
        {props.lastExecution.result.Result && (
          <ReactJson
            src={props.lastExecution.result.Result}
            theme="solarized"
            name={null}
          />
        )}
        {!props.lastExecution.result.Result && (
          <h1
            style={{
              color: "#fff"
            }}
          >
            SUCCESS
          </h1>
        )}
      </div>
    </div>
  );
}

class Commands extends Component {
  constructor(props) {
    super(props);

    this.state = {
      commands: [],
      displayCommands: [],
      selectedCommand: null,
      lastExecution: null,
      payload: null,
      currentFilter: ""
    };

    this.execute = this.execute.bind(this);
    this.filterCommands = this.filterCommands.bind(this);
    this.selectCommand = this.selectCommand.bind(this);
    this.updatePayload = this.updatePayload.bind(this);
  }

  componentDidMount() {
    apolloServer.invoke("GetAvailableCommands", {}).then(commands => {
      this.setState({
        commands,
        displayCommands: commands
      });
    });
  }

  selectCommand(commandListItem) {
    apolloServer
      .invoke("GetExamplePayload", {
        command: commandListItem.label
      })
      .then(payload => {
        this.setState({
          selectedCommand: commandListItem.label,
          payload: JSON.stringify(payload, null, 2)
        });
      });
  }

  updatePayload(e) {
    this.setState({
      payload: e.target.value
    });
  }

  execute(command, parameters) {
    apolloServer.invokeFull(command, JSON.parse(parameters)).then(result => {
      this.setState({
        lastExecution: result
      });
    });
  }

  filterCommands(e) {
    var currentFilter = e.target.value;
    if (currentFilter.length > 0) {
      var displayCommands = this.state.commands.filter(c => {
        return c.includes(currentFilter);
      });
      this.setState({
        displayCommands,
        currentFilter
      });
    } else {
      this.setState({
        displayCommands: this.state.commands,
        currentFilter
      });
    }
  }

  render() {
    const commandListing = this.state.displayCommands.map(cmd => {
      return {
        id: cmd,
        label: cmd
      };
    });
    return (
      <FlexRow>
        <Container>
          <input
            type="text"
            onChange={this.filterCommands}
            value={this.state.currentFilter}
            placeholder="filter commands"
          />
          <SelectList
            items={commandListing}
            onSelectItem={this.selectCommand}
          />
        </Container>
        <Container grow>
          {this.state.selectedCommand && (
            <CommandInfo
              payload={this.state.payload}
              command={this.state.selectedCommand}
              execute={this.execute}
              updatePayload={this.updatePayload}
            />
          )}
          {this.state.lastExecution && (
            <CommandResult lastExecution={this.state.lastExecution} />
          )}
        </Container>
      </FlexRow>
    );
  }
}

export default Commands;
