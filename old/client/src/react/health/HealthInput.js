import React from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { metricsActions, } from '../../redux/actions'
import { metricsSelectors, } from '../../redux/selectors'
import { NotifySuccess, } from '../../services/notifier'
import { SaveButton, FlexRow, } from '../_controls'
import './healthinput.css'

class Health extends React.Component {
  constructor(props) {
    super(props)
    this.state = this.buildInitialState()
  }

  buildInitialState() {
    const { metricsToRecord, } = this.props
    return metricsToRecord.reduce((acc, i) => {
      acc[i.name] = ''
      return acc
    }, {})
  }

  handleChange(name, value) {
    this.setState({ [name]: value, })
  }

  async save() {
    const { addMetrics, metricsToRecord, } = this.props
    const metricsWithValues = metricsToRecord
      .filter(m => this.state[m.name] !== '')
      .map(m => {
        return { name: m.name, value: this.state[m.name], category: 'health', }
      })

    await addMetrics(metricsWithValues)
    NotifySuccess('Health information successfully saved')
    this.setState(this.buildInitialState())
  }

  render() {
    const { metricsToRecord, } = this.props
    return (
      <fieldset>
        <legend>Daily Health Sheet</legend>
        <FlexRow wrap>
          {metricsToRecord.map(m => {
            return (
              <div className="metricInput" key={m.name}>
                <label>{m.label}</label>
                <input
                  type="text"
                  onChange={e => this.handleChange(m.name, e.target.value)}
                  value={this.state[m.name]}
                />
              </div>
            )
          })}
        </FlexRow>
        <SaveButton onClick={() => this.save()} />
      </fieldset>
    )
  }
}

Health.propTypes = {
  addMetrics: PropTypes.func.isRequired,
  metricsToRecord: PropTypes.array.isRequired,
}

function mapStateToProps() {
  return {
    metricsToRecord: metricsSelectors.healthMetrics(),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    addMetrics: metricInfo => dispatch(metricsActions.addMetrics(metricInfo)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Health)
