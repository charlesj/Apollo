import React, { Component } from "react";
import FontAwesome from "react-fontawesome";
import { FlexRow } from "../_controls";
import "./QuickNavigator.css";

class QuickNavigator extends Component {
  constructor(props) {
    super(props);

    this.state = {
      displayCommands: props.commands,
      filter: ""
    };
  }

  componentDidMount() {
    this.setState({ filter: "" });
  }

  handleChange(e) {
    const { commands } = this.props;
    const filter = e.target.value;
    if (filter.length > 0) {
      this.setState({
        filter,
        displayCommands: commands.filter(c => c.command.includes(filter))
      });
    } else {
      this.setState({
        filter,
        displayCommands: commands
      });
    }
  }

  handleKeyPress({ key }) {
    const { displayCommands } = this.state;
    if (key === "Enter" && displayCommands.length > 0) {
      displayCommands[0].execute();
    }
  }

  render() {
    const { filter, displayCommands } = this.state;
    return (
      <div className="quickNavigatorContainer">
        <div>
          <input
            type="text"
            ref={input => input && input.focus()}
            value={filter}
            onChange={e => this.handleChange(e)}
            onKeyPress={e => this.handleKeyPress(e)}
          />
        </div>
        <FlexRow wrap>
          {displayCommands.map(cmd => {
            return (
              <div
                className="quickCommand"
                key={cmd.command}
                onClick={() => cmd.execute()}
              >
                <FontAwesome name={cmd.icon} />
                {cmd.label}
              </div>
            );
          })}
        </FlexRow>
      </div>
    );
  }
}

export default QuickNavigator;
