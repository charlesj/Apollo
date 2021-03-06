import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { NotifySuccess, } from '../../services/notifier'
import { noteActions, } from '../../redux/actions'
import { noteSelectors, } from '../../redux/selectors'

import { Page, FlexRow, Container, AddButton, SelectList, } from '../_controls'
import NoteForm from './NoteForm'

import './Notes.css'

class Notebooks extends Component {
  constructor(props) {
    super(props)

    this.state = {
      note: null,
    }
  }

  componentDidMount() {
    this.props.getNotes()
  }

  selectNote(note) {
    this.setState({ note, })
  }

  saveNote(note) {
    const { saveNote, } = this.props
    saveNote(note)
    NotifySuccess('Successfully saved note')
  }

  render() {
    const { notes, } = this.props
    const selectedNote = this.state.note
    return (
      <Page>
        <div>
          Total notes: {notes.length}
          <AddButton onClick={() => this.selectNote({})} noun="New Note" />
        </div>
        <FlexRow>
          <div>
            <Container width={200}>
              <SelectList
                items={notes}
                onSelectItem={note => this.selectNote(note)}
                labelField="name"
              />
            </Container>
          </div>
          <div className="noteFormContainer">
            {selectedNote && (
              <Container>
                <NoteForm
                  note={selectedNote}
                  onSubmit={note => this.saveNote(note)}
                  onCancel={() => this.selectNote(null)}
                />
              </Container>
            )}
          </div>
        </FlexRow>
      </Page>
    )
  }
}

Notebooks.propTypes = {
  getNotes: PropTypes.func.isRequired,
  saveNote: PropTypes.func.isRequired,
  notes: PropTypes.array.isRequired,
}

function mapStateToProps(state) {
  return {
    notes: noteSelectors.all(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    getNotes: () => dispatch(noteActions.getAll()),
    saveNote: note => dispatch(noteActions.saveNote(note)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Notebooks)
