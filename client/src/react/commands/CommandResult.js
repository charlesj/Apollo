import React, { Component } from "react";
import ReactJson from "react-json-view";
import ClassNames from "classnames";
import FontAwesome from "react-fontawesome";
import { TextButton, Container, FlexRow, FlexContainer } from "../_controls";

export default class CommandResult extends Component {
  constructor(props) {
    super(props);
    this.state = {
      collapsed: false
    };
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.index > 0) {
      this.setState({ collapsed: true });
    }
  }

  render() {
    const { result } = this.props;
    const { collapsed } = this.state;
    console.log(result);
    if (!result.result) {
      return (
        <Container className="commandExecutionResult">
          <div className="commandExecutionHeader commandExecutionHeader-Error">
            ERROR {result.command}
          </div>
          <ReactJson src={result} theme="solarized" name={null} />
        </Container>
      );
    }
    return (
      <Container className="commandExecutionResult">
        <div
          className={ClassNames({
            commandExecutionHeader: true,
            "commandExecutionHeader-Success":
              result.result && result.result.ResultStatus === "Success",
            "commandExecutionHeader-Error": result.error
          })}
        >
          {result.result.ResultStatus.toUpperCase()} {result.id}:{" "}
          {result.command} ({result.result.Elapsed}ms)
          <TextButton onClick={() => this.setState({ collapsed: !collapsed })}>
            {collapsed && <FontAwesome name="ellipsis-h" />}
            {!collapsed && <FontAwesome name="ellipsis-v" />}
          </TextButton>
        </div>
        {!collapsed && (
          <FlexRow>
            {result.error && (
              <FlexContainer className="commandExecutionInfo">
                {result.error}: {result.result.ErrorMessage}
              </FlexContainer>
            )}
            <FlexContainer className="commandExecutionInfo">
              Result
              {result.result.Result && (
                <ReactJson
                  src={result.result.Result}
                  theme="solarized"
                  name={null}
                />
              )}
            </FlexContainer>
            <FlexContainer className="commandExecutionInfo">
              Parameters
              <ReactJson
                src={result.parameters}
                theme="solarized"
                name={null}
              />
            </FlexContainer>
          </FlexRow>
        )}
      </Container>
    );
  }
}
