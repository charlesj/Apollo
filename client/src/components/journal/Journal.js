var _ = require('lodash');
var React = require('react');
var PropTypes = require('prop-types');
var moment = require('moment');

var apollo = require('../../services/apollo-server');

class EntryInput extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      note: '',
      newTag: '',
      tags: []
    }

    this.handleChange = this.handleChange.bind(this);
    this.handleTagChange = this.handleTagChange.bind(this);
    this.addTag = this.addTag.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.removeTag = this.removeTag.bind(this);
  }

  handleChange(event) {
    this.setState({
      note: event.target.value
    });
  }

  handleTagChange(event){
      this.setState({
          newTag: event.target.value
      });
  }

  handleSubmit(event) {
    event.preventDefault();

    this.props.onSubmit(this.state.note, this.state.tags);
    this.setState({
      note: ''
    });
  }

  addTag(){
      var currentTags = this.state.tags;
      currentTags.push(this.state.newTag);
      this.setState({
          newTag:'',
          tags: currentTags
      });
  }

  removeTag(tag){
      var currentTags = this.state.tags;
       _.remove(currentTags, t =>{ return t === tag });
      this.setState({
          newTag:'',
          tags: currentTags
      });
  }

  render() {
    return (<form onSubmit={this.handleSubmit}>
      <div className="form-group">
        <lable htmlFor='note'>Add Note</lable>
        <textarea
      id='note'
      className='form-control'
      value={this.state.note}
      onChange={this.handleChange}>
        </textarea>
        </div>
      <div className="form-group">
        <lable htmlFor='tag'>Add Note</lable>
        <input
            id='tag'
            className='form-control'
            value={this.state.newTag}
            onChange={this.handleTagChange} />
        <button className='btn btn-sm' onClick={this.addTag} type='button'>Add Tag</button>
      </div>
      <div>
          Tags: <ul>
              {this.state.tags.map((tag) =>{
                  return(<li key={tag}>{tag} <button type='button' className='btn btn-sm' onClick={this.removeTag.bind(null, tag)}>X</button></li>)
              })}
          </ul>
      </div>
      <button className='btn btn-primary btn-sm' type='submit'>Add</button>
    </form>);
  }
}

EntryInput.propTypes = {
  onSubmit: PropTypes.func.isRequired
}

function EntryDisplay(props) {
  var createTime = moment(props.createdAt);
  return (<div className="note">
            <div className="createdTime">
              { createTime.format('Y-MM-DD HH:mm:SS') }
            </div>
            <div className="content">
              { props.note }
            </div>
        </div>)
}

EntryDisplay.propTypes = {
  createdAt: PropTypes.string.isRequired,
  note: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired
}

class Journal extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      entries: []
    }

    this.loadEntries = this.loadEntries.bind(this);
    this.addNewNote = this.addNewNote.bind(this);
  }

  componentDidMount() {
    this.loadEntries();
  }

  loadEntries() {
    this.setState(() => {
      return {
        entries: []
      }
    });

    apollo.invoke('GetAllJournalEntries', {})
      .then(data => {
        this.setState(() => {
          return {
            entries: data
          }
        })
      });
  }

  addNewNote(note, tags) {
    apollo.invoke('AddJournalEntry', {
      note: note,
      tags: tags
    }).then(() => {
      this.loadEntries();
    });
  }

  render() {
    return (
      <div className='container-fluid'>
        <h2>Journal</h2>

        <EntryInput onSubmit={this.addNewNote} />

        { this.state.entries.map(function(entry, index) {
        return (<EntryDisplay
          id={entry.id}
          key={entry.id}
          note={entry.note}
          createdAt={entry.created_at}
          />)
      }, this)}
      </div>);
  }
}


module.exports = Journal;
