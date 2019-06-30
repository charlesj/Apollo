import React, { Component, } from 'react'
import FontAwesome from 'react-fontawesome'
import PropTypes from 'prop-types'
import ClassNames from 'classnames'
import { Container, TextButton, } from '../_controls'
import ReactJson from 'react-json-view'

class ExecutionDisplay extends Component {
  constructor(props) {
    super(props)
    this.state = { collapsed: true, }
  }

  render() {
    const { exec, } = this.props
    const { collapsed, } = this.state
    return (
      <Container className="executionDisplay">
        <div
          className={ClassNames({
            executionHeader: true,
            'executionHeader-Success': exec.result_type === 'Success',
            'executionHeader-Error': exec.result_type !== 'Success',
          })}
        >
          {exec.result_type.toUpperCase()} {exec.executionTimeDisplay} ({
            exec.executionLength
          }ms)
          <TextButton onClick={() => this.setState({ collapsed: !collapsed, })}>
            {collapsed && <FontAwesome name="ellipsis-h" />}
            {!collapsed && <FontAwesome name="ellipsis-v" />}
          </TextButton>
        </div>
        {!collapsed && (
          <div className="executionResults">
            <ReactJson
              src={JSON.parse(exec.results)}
              theme="solarized"
              name={null}
            />
          </div>
        )}
      </Container>
    )
  }
}

ExecutionDisplay.propTypes = {
  exec: PropTypes.object.isRequired,
}

function JobDetails(props) {
  const { details: { job, history, }, } = props

  return (
    <Container className="jobDetails">
      <h3>Job: {job.command_name}</h3>
      <div className="jobSchedule">
        <span className="scheduleComponent">Started: {job.startDisplay}</span>
        <span className="scheduleComponent">
          Runs: {job.scheduleJson.minutely && 'minutely'}
          {job.scheduleJson.hourly && 'hourly'}
          {job.scheduleJson.daily && 'daily'}
        </span>
        <span className="scheduleComponent">
          Repeats:{' '}
          {job.scheduleJson.repeat_count ? (
            `${job.scheduleJson.repeat_count} times`
          ) : (
            <span>&infin;</span>
          )}
        </span>
      </div>
      <div className="jobParameters">
        <ReactJson src={job.parametersJson} theme="solarized" name={null} />
      </div>
      {history.map(exec => {
        return <ExecutionDisplay key={exec.id} exec={exec} />
      })}
    </Container>
  )
}

JobDetails.propTypes = {
  details: PropTypes.object.isRequired,
}

export default JobDetails
