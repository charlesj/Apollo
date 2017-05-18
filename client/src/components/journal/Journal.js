var _ = require('lodash');
var React = require('react');
var PropTypes = require('prop-types');
var moment = require('moment');

import { InputGroup, EditableText } from "@blueprintjs/core";

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
    this.handleTagKeyPress = this.handleTagKeyPress.bind(this);
    this.addTag = this.addTag.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.removeTag = this.removeTag.bind(this);
  }

  handleChange(change) {
    this.setState({
      note: change
    });
  }

  handleTagChange(event) {
    this.setState({
      newTag: event.target.value
    });
  }

  handleTagKeyPress(target) {
    if (target.charCode === 13) {
      this.addTag();
    }
  }

  handleSubmit(event) {
    event.preventDefault();

    this.props.onSubmit(this.state.note, this.state.tags);
    this.setState({
      note: '',
      tags: []
    });
  }

  addTag() {
    var currentTags = this.state.tags;
    currentTags.push(this.state.newTag);
    this.setState({
      newTag: '',
      tags: currentTags
    });
  }

  removeTag(tag) {
    var currentTags = this.state.tags;
    _.remove(currentTags, t => {
      return t === tag
    });
    this.setState({
      newTag: '',
      tags: currentTags
    });
  }

  render() {
    return (<div>
      <div className="pt-form-group">
        <EditableText

      maxLines={12}
      minLines={7}
      multiline
      placeholder="Add note"
      selectAllOnFocus={false}
      confirmOnEnterKey={false}
      value={this.state.note}
      onChange={this.handleChange}
      />
      </div>
      <div className="pt-form-group">
        <InputGroup
      leftIconName="tag"
      onChange={this.handleTagChange}
      placeholder="Add tags"
      value={this.state.newTag}
      onKeyPress={this.handleTagKeyPress}
      />
      </div>
      <div>
              {this.state.tags.map((tag) => {
        return (<span className='pt-tag pt-tag-removable' key={tag}>
                        {tag}
                        <button className='pt-tag-remove' type='button' onClick={this.removeTag.bind(null, tag)}></button></span>)
      })}

      </div>
      <div>
        <button type='submit' className='pt-button pt-intent-primary pt-icon-add' onClick={this.handleSubmit}>Save Note</button>
      </div>
  </div>);
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
