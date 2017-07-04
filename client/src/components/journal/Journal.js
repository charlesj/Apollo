import _ from 'lodash';
import React from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
import { Tab2, Tabs2 } from "@blueprintjs/core";
import MarkdownRenderer from 'react-markdown-renderer';
import apollo from '../../services/apollo-server';
import EntryInput from './EntryInput';

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
