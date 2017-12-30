import React from "react";
import moment from "moment";
import apolloServer from "../../services/apolloServer";

import JobHistory from "./JobHistory";

class Jobs extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      jobs: [],
      historyJob: null,
      jobFilter: "active"
    };

    this.loadActiveJobs = this.loadActiveJobs.bind(this);
    this.setHistoryJob = this.setHistoryJob.bind(this);
    this.cancelJob = this.cancelJob.bind(this);
    this.jobFilterChange = this.jobFilterChange.bind(this);
  }

  componentDidMount() {
    this.loadActiveJobs();
  }

  loadActiveJobs(expired) {
    apolloServer
      .invoke("getJobs", {
        expired
      })
      .then(jobs => {
        this.setState({
          jobs
        });
      });
  }

  cancelJob(job) {
    alert("Not Implemented");
  }

  setHistoryJob(job) {
    if (this.state.historyJob === job) {
      this.setState({
        historyJob: null
      });
      return;
    }

    this.setState({
      historyJob: job
    });
  }

  jobFilterChange(e) {
    this.setState({
      jobFilter: e.target.value
    });
    this.loadActiveJobs(e.target.value === "expired");
  }

  render() {
    if (this.state.jobs.length === 0) {
      return <div>No Active Jobs</div>;
    }
    return (
      <div>
        <div className="pt-select pt-minimal">
          <select onChange={this.jobFilterChange} value={this.state.jobFilter}>
            <option value="active">active</option>
            <option value="expired">expired</option>
          </select>
        </div>
        <table className="pt-table pt-bordered">
          <thead>
            <tr>
              <th>id</th>
              <th>command</th>
              <th>started</th>
              <th>last executed</th>
              <th />
              <th />
            </tr>
          </thead>
          <tbody>
            {this.state.jobs.map(job => {
              var createdAt = moment(job.created_at);
              var lastExecuted = moment(job.last_executed_at);
              return (
                <tr key={job.id}>
                  <td>{job.id}</td>
                  <td>{job.command_name}</td>
                  <td>{createdAt.calendar()}</td>
                  <td>{lastExecuted.calendar()}</td>
                  <td>
                    <button
                      className="pt-button pt-small pt-intent-success"
                      onClick={this.setHistoryJob.bind(null, job)}
                    >
                      history
                    </button>
                    <button
                      className="pt-button pt-small pt-intent-danger"
                      onClick={this.cancelJob.bind(null, job)}
                    >
                      cancel
                    </button>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
        <JobHistory job={this.state.historyJob} />
      </div>
    );
  }
}

export default Jobs;
