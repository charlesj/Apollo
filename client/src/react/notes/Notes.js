import React, { Component } from "react";
import { connect } from "react-redux";
import { NotifySuccess } from "../../services/notifier";
import ClassNames from "classnames";
import { noteActions } from "../../redux/actions";
import { noteSelectors } from "../../redux/selectors";

import { Page, FlexRow, Container, AddButton } from "../_controls";
import NoteForm from "./NoteForm";

import "./Notes.css";

function NoteListingDisplay(props) {
  const { notes, selectedNote, onSelectNote } = props;

  return (
    <div className="notesListings">
      {notes.map((n, i) => {
        const isSelected = selectedNote && n.id === selectedNote.id;
        return (
          <div
            key={i}
            className={ClassNames({
              noteListing: true,
              selected: isSelected
            })}
            onClick={() => onSelectNote(n)}
          >
            {n.name}
          </div>
        );
      })}
    </div>
  );
}

class Notebooks extends Component {
  constructor(props) {
    super(props);

    this.state = {
      note: null
    };
  }

  componentDidMount() {
    this.props.getNotes();
  }

  selectNote(note) {
    this.setState({ note });
  }

  saveNote(note) {
    console.log("submit", note);
    const { saveNote } = this.props;
    saveNote(note);
    NotifySuccess("Successfully saved note");
  }

  render() {
    const { notes } = this.props;
    const selectedNote = this.state.note;
    return (
      <Page>
        <div>
          Total notes: {notes.length}
          <AddButton onClick={() => this.selectNote({})} noun="New Note" />
        </div>
        <FlexRow>
          <div>
            <Container width={200}>
              <NoteListingDisplay
                selectedNote={selectedNote}
                notes={notes}
                onSelectNote={note => this.selectNote(note)}
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
    );
  }
}

function mapStateToProps(state, props) {
  return {
    notes: noteSelectors.all(state)
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    getNotes: () => dispatch(noteActions.getAll()),
    saveNote: note => dispatch(noteActions.saveNote(note))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Notebooks);
