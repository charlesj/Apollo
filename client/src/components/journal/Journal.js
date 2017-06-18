var _ = require('lodash');
var React = require('react');
var PropTypes = require('prop-types');
var moment = require('moment');
import { Tab2, Tabs2 } from "@blueprintjs/core";
import { InputGroup, EditableText } from "@blueprintjs/core";
import MarkdownRenderer from 'react-markdown-renderer';

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
        <div className="pt-card">
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
</div>
        <button type='submit' className='pt-button pt-intent-primary pt-icon-add buttonSpace' onClick={this.handleSubmit}>Save Note</button>

  </div>);
  }
}

EntryInput.propTypes = {
  onSubmit: PropTypes.func.isRequired
}

function EntryDisplay(props) {
  var createTime = moment(props.createdAt);
  return (<div className="note">
      <span className="pt-icon-standard pt-icon-document"></span>
            <div className="createdTime">

              { createTime.calendar() }
            </div>
            <div className="content pt-card">
              <MarkdownRenderer markdown={ props.note } />
            </div>
            <div className="tags">
                { props.tags && props.tags.map((t, i) => {
                    return (<span key={i} className="pt-tag">{t}</span>)
                })}
            </div>
        </div>)
}

EntryDisplay.propTypes = {
  createdAt: PropTypes.string.isRequired,
  note: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired
}

function EntryListDisplay(props) {
  return ( <div>       { props.entries.map(function(entry, index) {
      return (<EntryDisplay
        id={entry.id}
        key={entry.id}
        note={entry.note}
        createdAt={entry.created_at}
        tags={entry.tags}
        />)
    })}
      </div>)
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
        <Tabs2 id="JournalTabs" onChange={this.handleTabChange}>
            <Tab2 id="new_entry" title="Entries" panel={<EntryListDisplay entries={this.state.entries} />} />
            <Tab2 id="entries" title="Add Entry" panel={<EntryInput onSubmit={this.addNewNote} />} />
            <Tabs2.Expander />
        </Tabs2>
      </div>);
  }
}


module.exports = Journal;
