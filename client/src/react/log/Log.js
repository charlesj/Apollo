import React from "react";
import PropTypes from "prop-types";
import moment from "moment";
import MarkdownRenderer from "react-markdown-renderer";
import apollo from "../../services/apolloServer";
import EntryInput from "./EntryInput";
import { NotifySuccess } from "../../services/notifier";
import "./logs.css";

function EntryDisplay(props) {
  var createTime = moment(props.createdAt);
  return (
    <div className="note">
      <span className="pt-icon-standard pt-icon-document" />
      <div className="createdTime">{createTime.calendar()}</div>
      <div className="content pt-card">
        <MarkdownRenderer markdown={props.note} />
      </div>
      <div className="tags">
        {props.tags &&
          props.tags.map((t, i) => {
            return (
              <span key={i} className="pt-tag">
                {t}
              </span>
            );
          })}
      </div>
    </div>
  );
}

EntryDisplay.propTypes = {
  createdAt: PropTypes.string.isRequired,
  note: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired
};

function EntryListDisplay(props) {
  return (
    <div>
      {" "}
      {props.entries.map(function(entry, index) {
        return (
          <EntryDisplay
            id={entry.id}
            key={entry.id}
            note={entry.note}
            createdAt={entry.created_at}
            tags={entry.tags}
          />
        );
      })}
    </div>
  );
}

class Journal extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      entries: []
    };

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
      };
    });

    apollo.invoke("GetAllJournalEntries", {}).then(data => {
      this.setState(() => {
        return {
          entries: data
        };
      });
    });
  }

  addNewNote(note, tags) {
    apollo
      .invoke("AddJournalEntry", {
        note: note,
        tags: tags
      })
      .then(() => {
        NotifySuccess("Successfully added a new log entry.");
        this.loadEntries();
      });
  }
  log;
  render() {
    return (
      <div>
        <EntryInput onSubmit={this.addNewNote} />
        <EntryListDisplay entries={this.state.entries} />
      </div>
    );
  }
}

export default Journal;
