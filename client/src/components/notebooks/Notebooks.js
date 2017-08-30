import React from 'react';
import apolloServer from '../../services/apollo-server';
import { Intent } from "@blueprintjs/core";
import { Notifier } from '../../services/notifier';

function getNoteListingClasses(selectedId, feedId) {
  if (selectedId !== feedId) {
    return "noteListing";
  }

  return "noteListing activeNote";
}

function NoteListingDisplay(props) {

  return (<div className='notesListings'>
    <div className={getNoteListingClasses(props.selectedId, -1)} onClick={props.onChangeNote.bind(null, -1)}>New Note</div>
    { props.notes.map(n => {
      return (<div className={getNoteListingClasses(props.selectedId, n.id)} key={n.id} onClick={props.onChangeNote.bind(null, n.id)}>{n.name}</div>)
    })}
  </div>)
}

class Notebooks extends React.Component {
  constructor(props){
    super(props);

    this.state = {
      notes: [],
      selectedNoteId: -1,
      noteName: '',
      noteBody: ''
    };

    this.changeSelectedNote = this.changeSelectedNote.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.loadNotes = this.loadNotes.bind(this);
    this.save = this.save.bind(this);
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  componentDidMount(){
    this.loadNotes();
  }

  loadNotes(){
    apolloServer.invoke('getNotes', {})
      .then(notes => {
        this.setState({notes});
      });
  }

  changeSelectedNote(id){
    if(id === -1){
      this.setState({
        selectedNoteId: -1,
        noteName: "",
        noteBody: ""
      });
      return;
    };

    apolloServer.invoke('getNote', {id})
      .then(note => {
        this.setState({
          selectedNoteId: id,
          noteName: note.name,
          noteBody: note.body
        });
    });
  }

  save(){
    if(this.state.selectedNoteId === -1){
      apolloServer.invoke('addNote', {name: this.state.noteName, body: this.state.noteBody})
        .then(() => {
          Notifier.show({
            intent: Intent.SUCCESS,
            message: "Successfully saved a new note.",
          });
          this.setState({
            selectedNoteId: -1,
            noteName: '',
            noteBody: ''
          })
          this.loadNotes();
        });
    } else {
      apolloServer.invoke('updateNote', {
        id: this.state.selectedNoteId,
        name: this.state.noteName,
        body: this.state.noteBody
      }).then(() => {
          Notifier.show({
            intent: Intent.SUCCESS,
            message: "Successfully updated note.",
          });
      });
    }
  }

  render(){
    return (<div className='notebooksContainer'>
      <NoteListingDisplay
        selectedId={this.state.selectedNoteId}
        notes={this.state.notes}
        onChangeNote={this.changeSelectedNote} />
      <div className='notebookDisplay'>
        <div className='notebookCommandBar'>
          <button className='pt-button pt-intent-primary' onClick={this.save}>Save</button>
        </div>
        <div>
          <input
            type="text"
            name="noteName"
            className='pt-input pt-fill'
            placeholder='note name'
            onChange={this.handleChange}
            value={this.state.noteName} />
          <textarea name="noteBody" className='pt-input' onChange={this.handleChange} value={this.state.noteBody}></textarea>
        </div>
      </div>
    </div>)
  }
}

module.exports = Notebooks;
