import React, { Component } from "react";
import moment from "moment";

import apolloServer from "../../services/apolloServer";

class ExecutionDisplay extends Component {
  constructor(props) {
    super(props);
    this.state = {
      showResult: false,
      exe: props.exe
    };

    this.handleClick = this.handleClick.bind(this);
  }

  handleClick() {
    this.setState({
      showResult: !this.state.showResult
    });
  }

  render() {
    var executedAt = moment(this.state.exe.executed_at);
    return (
      <div className="executionHistory">
        {executedAt.calendar()} |
        {this.state.exe.result_type}
        |
        <button onClick={this.handleClick}>
          {this.state.showResult ? "Hide" : "Show"} Result
        </button>
        {this.state.showResult && (
          <pre>
            {JSON.stringify(JSON.parse(this.state.exe.results), null, 2)}
          </pre>
        )}
      </div>
    );
  }
}

class JobHistory extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      history: []
    };

    this.loadJobHistory = this.loadJobHistory.bind(this);
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.job) {
      this.loadJobHistory(nextProps.job);
    }
  }

  loadJobHistory(job) {
    apolloServer
      .invoke("getJobHistory", {
        jobId: job.id
      })
      .then(history => {
        this.setState({
          history
        });
      });
  }

  render() {
    if (!this.props.job) {
      return <div />;
    }

    return (
      <div>
        <hr />
        <h3>
          {this.props.job.command_name}:{this.props.job.id}
        </h3>
        {this.state.history.map(je => {
          return <ExecutionDisplay exe={je} key={je.id} />;
        })}
      </div>
    );
  }
}

export default JobHistory;
