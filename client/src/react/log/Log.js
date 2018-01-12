import React from "react";
import { connect } from "react-redux";
import MarkdownRenderer from "react-markdown-renderer";

import { logActions } from "../../redux/actions";
import { logSelectors } from "../../redux/selectors";
import { NotifySuccess } from "../../services/notifier";
import { Page, LoadMoreButton, Tag, AddButton } from "../_controls";
import LogEntryForm from "./LogEntryForm";

import "./logs.css";

function EntryDisplay(props) {
  const { entry } = props;
  return (
    <div className="note">
      <div className="createdTime">{entry.createdAtDisplay}</div>
      <div className="content">
        <MarkdownRenderer markdown={entry.note} />
      </div>
      <div className="tags">
        {entry.tags &&
          entry.tags.map((t, i) => {
            return <Tag key={i} name={t} />;
          })}
      </div>
    </div>
  );
}

function EntryListDisplay(props) {
  return (
    <div>
      {props.entries.map(function(entry, index) {
        return <EntryDisplay entry={entry} key={entry.id} />;
      })}
    </div>
  );
}

class Journal extends React.Component {
  constructor(props) {
    super(props);
    this.state = { addNote: true };
  }
  componentDidMount() {
    const { loadEntries, entries } = this.props;
    loadEntries(entries.length);
  }

  handleNewEntry(formResult) {
    const { saveEntry } = this.props;

    saveEntry({
      note: formResult.note,
      tags: formResult.unifiedTags.split(",")
    });

    NotifySuccess("Added note");
    this.setState({ addNote: false });
  }

  render() {
    const { entries, loadEntries, total } = this.props;
    const { addNote } = this.state;

    return (
      <Page>
        {!addNote && (
          <AddButton onClick={() => this.setState({ addNote: !addNote })} noun="Entry"/>
        )}
        {addNote && (
          <LogEntryForm
            onCancel={() => this.setState({ addNote: !addNote })}
            onSubmit={formResult => this.handleNewEntry(formResult)}
          />
        )}
        <EntryListDisplay entries={entries} />
        <div>
          {total} Total Entries {entries.length} Loaded
        </div>
        <LoadMoreButton onClick={() => loadEntries(entries.length)} />
      </Page>
    );
  }
}

function mapStateToProps(state, props) {
  const entries = logSelectors.all(state);

  return {
    entries,
    total: state.log.total
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    loadEntries: start => dispatch(logActions.load({ start })),
    saveEntry: entry => dispatch(logActions.save(entry))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Journal);
