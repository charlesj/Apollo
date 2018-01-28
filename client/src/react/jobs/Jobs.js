import React from "react";
import { connect } from "react-redux";
import { jobActions } from "../../redux/actions";
import { jobSelectors } from "../../redux/selectors";
import { Container, Button } from "../_controls";
import JobDetails from "./JobDetails";

import "./jobs.css";

class Jobs extends React.Component {
  componentDidMount() {
    this.props.load();
  }

  render() {
    const { jobs, selectJob, jobDetails } = this.props;
    if (jobs.length === 0) {
      return <Container>No Active Jobs</Container>;
    }
    return (
      <div>
        <Container>
          <table className="jobTable">
            <thead>
              <tr>
                <th>id</th>
                <th>command</th>
                <th>started</th>
                <th>last executed</th>
                <th>expired</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {jobs.map(job => {
                return (
                  <tr key={job.id} className="tableRow">
                    <td>{job.id}</td>
                    <td>{job.command_name}</td>
                    <td>{job.createdAtDisplay}</td>
                    <td>{job.lastExecutedDisplay}</td>
                    <td>{job.expiredAtDisplay || "active"}</td>
                    <td>
                      <Button onClick={() => selectJob(job)}>Details</Button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </Container>
        {jobDetails && <JobDetails details={jobDetails} />}
      </div>
    );
  }
}

function mapStateToProps(state, props) {
  return {
    jobs: jobSelectors.all(state),
    jobDetails: jobSelectors.jobDetails(state)
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    load: () => dispatch(jobActions.load()),
    selectJob: job => dispatch(jobActions.selectJob(job))
  };
}
export default connect(mapStateToProps, mapDispatchToProps)(Jobs);
