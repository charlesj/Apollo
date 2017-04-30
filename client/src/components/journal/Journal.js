var React = require('react');
var PropTypes = require('prop-types');
var moment = require('moment');

var apollo = require('../../services/apollo-server');

class EntryInput extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      note: ''
    }

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    this.setState({
      note: event.target.value
    });
  }

  handleSubmit(event) {
    event.preventDefault();

    this.props.onSubmit(this.state.note);
    this.setState({
      note: ''
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

  addNewNote(note) {
    apollo.invoke('AddJournalEntry', {
      note: note
    }).then(() => {
      this.loadEntries();
    });
  }

  render() {
    return (
      <div>
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
