import React from 'react'
import { connect, } from 'react-redux'
import moment from 'moment'
import PropTypes from 'prop-types'
import { checklistSelectors, } from '../../redux/selectors'
import { checklistActions, } from '../../redux/actions'

import { Container, } from '../_controls'

class ChecklistCompletionLog extends React.Component {
  componentDidMount() {
    this.props.load()
  }

  render() {
    const { log, } = this.props
    return (
      <Container width={350} className="completionLogContainer">
        <h1>Completion Log</h1>
        {log.map(c => {
          let completed_at = moment(c.completed_at)
          return (
            <div className="completionLogEntry" key={c.completion_id}>
              {completed_at.calendar()}: {c.name}
            </div>
          )
        })}
      </Container>
    )
  }
}

ChecklistCompletionLog.propTypes = {
  log: PropTypes.array.isRequired,
  load: PropTypes.func.isRequired,
}

function mapStateToProps(state) {
  return {
    log: checklistSelectors.completionLog(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    load: () => dispatch(checklistActions.getChecklistCompletionLog()),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(
  ChecklistCompletionLog
)
