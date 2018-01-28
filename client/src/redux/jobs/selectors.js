import _ from "lodash";
import moment from "moment";

import { keyedIdToArray } from "../selector-helpers";

export function all(state) {
  const array = keyedIdToArray(state.jobs.jobs).map(job => {
    const scheduleJson = JSON.parse(job.schedule);
    return {
      ...job,
      scheduleJson,
      startDisplay: moment(scheduleJson.start).calendar(),
      createdAtDisplay: moment(job.created_at).calendar(),
      lastExecutedDisplay:
        job.last_executed_at && moment(job.last_executed_at).calendar(),
      expiredAtDisplay: job.expired_at && moment(job.expired_at).calendar(),
      parametersJson: JSON.parse(job.parameters)
    };
  });
  return _.orderBy(array, "created_at", "desc");
}

export function jobDetails(state) {
  if (!state.jobs.selectedJob) {
    return null;
  }
  const job = state.jobs.selectedJob;
  const historyArray = keyedIdToArray(state.jobs.jobHistories).filter(
    history => {
      return history.job_id === job.id;
    }
  );

  const history = _.orderBy(historyArray, "execution_ended", "desc").map(
    exec => {
      const execStarted = moment(exec.executed_at);
      const execEnded = moment(exec.execution_ended);
      return {
        ...exec,
        executionTimeDisplay: moment(exec.executed_at).calendar(),
        executionLength:
          exec.execution_ended &&
          moment.duration(execEnded - execStarted).milliseconds()
      };
    }
  );
  return {
    job,
    history
  };
}
