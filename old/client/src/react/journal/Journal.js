import React from 'react'
import { connect, } from 'react-redux'
import MarkdownRenderer from 'react-markdown-renderer'
import PropTypes from 'prop-types'
import { journalActions, } from '../../redux/actions'
import { journalSelectors, } from '../../redux/selectors'
import { NotifySuccess, } from '../../services/notifier'
import { Page, LoadMoreButton, Tag, AddButton, } from '../_controls'
import JournalEntryForm from './JournalEntryForm'

import './logs.css'

function EntryDisplay(props) {
  const { entry, } = props
  return (
    <div className="note">
      <div className="createdTime">{entry.createdAtDisplay}</div>
      <div className="content">
        <MarkdownRenderer markdown={entry.note} />
      </div>
      <div className="tags">
        {entry.tags &&
          entry.tags.map((t, i) => {
            return <Tag key={i} name={t} />
          })}
      </div>
    </div>
  )
}

EntryDisplay.propTypes = {
  entry: PropTypes.object.isRequired,
}

function EntryListDisplay(props) {
  return (
    <div>
      {props.entries.map((entry) => {
        return <EntryDisplay entry={entry} key={entry.id} />
      })}
    </div>
  )
}

EntryListDisplay.propTypes = {
  entries: PropTypes.array.isRequired,
}

class Journal extends React.Component {
  constructor(props) {
    super(props)
    this.state = { addNote: true, }
  }
  componentDidMount() {
    const { loadEntries, entries, } = this.props
    loadEntries(entries.length)
  }

  handleNewEntry(formResult) {
    const { saveEntry, } = this.props

    saveEntry({
      note: formResult.note,
      tags: formResult.unifiedTags.split(','),
    })

    NotifySuccess('Added note')
    this.setState({ addNote: false, })
  }

  render() {
    const { entries, loadEntries, total, } = this.props
    const { addNote, } = this.state

    return (
      <Page>
        {!addNote && (
          <AddButton
            onClick={() => this.setState({ addNote: !addNote, })}
            noun="Entry"
          />
        )}
        {addNote && (
          <JournalEntryForm
            onCancel={() => this.setState({ addNote: !addNote, })}
            onSubmit={formResult => this.handleNewEntry(formResult)}
          />
        )}
        <EntryListDisplay entries={entries} />
        <div>
          {total} Total Entries {entries.length} Loaded
        </div>
        <LoadMoreButton onClick={() => loadEntries(entries.length)} />
      </Page>
    )
  }
}

Journal.propTypes = {
  entries: PropTypes.array.isRequired,
  total: PropTypes.number.isRequired,
  loadEntries: PropTypes.func.isRequired,
  saveEntry: PropTypes.func.isRequired,
}

function mapStateToProps(state) {
  const entries = journalSelectors.all(state)

  return {
    entries,
    total: state.journal.total,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    loadEntries: start => dispatch(journalActions.load({ start, })),
    saveEntry: entry => dispatch(journalActions.save(entry)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Journal)
