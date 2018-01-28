import React, { Component } from "react";
import apolloServer from "../../services/apolloServer";
import {
  Button,
  Container,
  FlexRow,
  SelectList,
  FlexContainer
} from "../_controls";
import CommandResult from "./CommandResult";

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

class Commands extends Component {
  constructor(props) {
    super(props);

    this.state = {
      commands: [],
      displayCommands: [],
      selectedCommand: null,
      results: [],
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

  execute(command, params) {
    const parameters = JSON.parse(params);
    apolloServer.invokeFull(command, parameters).then(result => {
      this.setState({
        results: [
          { command, ...result, parameters, id: this.state.results.length + 1 },
          ...this.state.results
        ]
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
    const { results } = this.state;
    return (
      <FlexRow>
        <FlexContainer>
          <Container width={250}>
            <input
              type="text"
              onChange={this.filterCommands}
              value={this.state.currentFilter}
              placeholder="filter commands"
              className="commandFilterInput"
            />
            <SelectList
              items={commandListing}
              onSelectItem={this.selectCommand}
              labelField="label"
            />
          </Container>
        </FlexContainer>
        <FlexContainer grow>
          <Container grow>
            {this.state.selectedCommand && (
              <CommandInfo
                payload={this.state.payload}
                command={this.state.selectedCommand}
                execute={this.execute}
                updatePayload={this.updatePayload}
              />
            )}
          </Container>
          {results.map((result, i) => {
            return <CommandResult key={result.id} result={result} index={i} />;
          })}
        </FlexContainer>
      </FlexRow>
    );
  }
}

export default Commands;
