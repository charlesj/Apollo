import { createActions, } from 'redux-actions'
import { basicActions, dispatchBasicActions, } from '../redux-helpers'
import apolloServer from '../../services/apolloServer'

const actionCreators = createActions({
  jobs: {
    load: basicActions(),
    selectJob: basicActions(),
  },
})

export const actions = actionCreators.jobs

export function load() {
  return dispatchBasicActions(actions.load, async () => {
    const jobs = await apolloServer.invoke('getJobs', { expired: true, })
    return { jobs, }
  })
}

export function selectJob(job) {
  return dispatchBasicActions(actions.selectJob, async () => {
    const jobHistories = await apolloServer.invoke('getJobHistory', {
      jobId: job.id,
    })
    return { job, jobHistories, }
  })
}
