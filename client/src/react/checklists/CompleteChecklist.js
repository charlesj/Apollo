import React from 'react'
import { connect, } from 'react-redux'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'
import { checklistSelectors, } from '../../redux/selectors'
import { checklistActions, } from '../../redux/actions'
import { NotifySuccess, } from '../../services/notifier'

import {
  Container,
  FlexRow,
  TextButton,
  CancelButton,
  SaveButton,
  FlexContainer,
} from '../_controls'
import './CompleteChecklists.css'

class CompleteChecklist extends React.Component {
  constructor(props) {
    super(props)

    this.state = {
      checklistItems: [],
      notes: '',
    }
  }

  componentDidMount() {
    this.props.getChecklists()
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value,
    })
  }

  toggleChecklistItemComplete(checklist_item_id) {
    const { checklistItems, } = this.state
    const updated = { ...checklistItems, }
    const current = checklistItems[checklist_item_id]
    updated[checklist_item_id] = !current
    this.setState({ checklistItems: updated, })
  }

  handleSubmit() {
    const {
      selectedChecklist,
      addCompletedChecklist,
      selectChecklist,
    } = this.props
    const { notes, checklistItems, } = this.state
    const items = selectedChecklist.items.map(item => {
      return {
        checklist_item_id: item.id,
        completed: checklistItems[item.id] ? 1 : 0,
      }
    })
    addCompletedChecklist(selectedChecklist.id, notes, items)
    selectChecklist(null)
    NotifySuccess(`Committed ${selectedChecklist.name}`)
    this.setState({ notes: '', checklistItems: [], })
  }

  render() {
    const { checklists, selectedChecklist, selectChecklist, } = this.props
    const { checklistItems, notes, } = this.state
    return (
      <Container className="completeChecklists" width={400}>
        <FlexRow>
          {checklists.map(checklist => {
            return (
              <TextButton
                key={checklist.id}
                onClick={() => selectChecklist(checklist)}
              >
                {checklist.name}
              </TextButton>
            )
          })}
        </FlexRow>
        {selectedChecklist && (
          <FlexContainer className="checklistContainer">
            <div className="checklistName">{selectedChecklist.name}</div>
            <div className="checklistDescription">
              {selectedChecklist.description}
            </div>
            <div className="checklistItems">
              {selectedChecklist.items.map(item => {
                return (
                  <div
                    key={item.id}
                    className={ClassNames({
                      checklistItem: true,
                      [`checklistItem-${item.type}`]: true,
                    })}
                  >
                    <div className="checklistItemCheckbox">
                      <input
                        type="checkbox"
                        name={'item-' + item.id}
                        checked={!!checklistItems[item.id]}
                        onChange={() =>
                          this.toggleChecklistItemComplete(item.id)
                        }
                      />
                    </div>
                    <div className="checklistItemName">{item.name}</div>
                    <div className="checklistItemDescription">
                      {item.description}
                    </div>
                  </div>
                )
              })}
            </div>
            <div>
              <textarea
                name="notes"
                id="notes"
                onChange={e => this.handleChange(e)}
                value={notes}
                placeholder="enter any notes here"
              />
            </div>
            <CancelButton onClick={() => selectChecklist(null)} />
            <SaveButton onClick={() => this.handleSubmit()} />
          </FlexContainer>
        )}
      </Container>
    )
  }
}

CompleteChecklist.propTypes = {
  getChecklists: PropTypes.func.isRequired,
  selectedChecklist: PropTypes.object,
  addCompletedChecklist: PropTypes.func.isRequired,
  checklists: PropTypes.array.isRequired,
  selectChecklist: PropTypes.func.isRequired,
}

function mapStateToProps(state) {
  return {
    checklists: checklistSelectors.all(state),
    selectedChecklist: checklistSelectors.selectedChecklist(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    getChecklists: () => dispatch(checklistActions.getChecklists()),
    selectChecklist: checklist =>
      dispatch(checklistActions.default.selectChecklist(checklist)),
    addCompletedChecklist: (checklist_id, notes, items) =>
      dispatch(
        checklistActions.addCompletedChecklist(checklist_id, notes, items)
      ),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(CompleteChecklist)
