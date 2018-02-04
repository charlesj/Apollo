import React, { Component } from "react";
import { connect } from "react-redux";

import { timelineSelectors } from "../../redux/selectors";
import { timelineActions } from "../../redux/actions";

import { Container } from "../_controls";

class Timeline extends Component {
  componentDidMount() {
    this.props.load();
  }

  render() {
    const { entries } = this.props;

    return (
      <Container>
        Timeline
        {entries.map(entry => {
          return (
            <div>
              {entry.eventTimeDisplay}: {entry.title}
            </div>
          );
        })}
      </Container>
    );
  }
}

function mapStateToProps(state, props) {
  return {
    entries: timelineSelectors.all(state)
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    load: () => dispatch(timelineActions.load())
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Timeline);
