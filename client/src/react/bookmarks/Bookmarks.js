import React from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'
import { bookmarkActions, } from '../../redux/actions'
import { bookmarkSelectors, } from '../../redux/selectors'
import { NotifySuccess, } from '../../services/notifier'
import { AddButton, Page, } from '../_controls'
import BookmarksDisplay from './BookmarksDisplay'
import BookmarkForm from './BookmarkForm'

import './bookmarks.css'

class Bookmarks extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      editingBookmark: null,
    }
  }

  componentDidMount() {
    const { load, bookmarks, } = this.props
    load(bookmarks.length)
  }

  saveBookmark(bookmark) {
    const { save, } = this.props
    save(bookmark)
    this.setState({ editingBookmark: null, })
    NotifySuccess('Bookmark Saved')
  }

  editBookmark(bookmark) {
    this.setState({ editingBookmark: bookmark, })
  }

  render() {
    const { bookmarks, total, load, remove, } = this.props
    const { editingBookmark, } = this.state
    return (
      <Page>
        <div>
          Total bookmarks: {total}
          {!editingBookmark && (
            <AddButton onClick={() => this.editBookmark({})} noun="Bookmark" />
          )}
        </div>
        {editingBookmark && (
          <BookmarkForm
            bookmark={editingBookmark}
            onSubmit={data => this.saveBookmark(data)}
            onCancel={() => this.editBookmark(null)}
          />
        )}
        <BookmarksDisplay
          bookmarks={bookmarks}
          totalBookmarks={total}
          loadBookmarks={load}
          refreshBookmarks={load}
          editBookmark={bookmark => this.editBookmark(bookmark)}
          deleteBookmark={bookmark => {
            remove(bookmark)
            NotifySuccess('Bookmark Removed')
          }}
        />
      </Page>
    )
  }
}

Bookmarks.propTypes = {
  bookmarks: PropTypes.array.isRequired,
  total: PropTypes.number.isRequired,
  load: PropTypes.func.isRequired,
  save: PropTypes.func.isRequired,
  remove: PropTypes.func.isRequired,
}

function mapStateToProps(state) {
  const bookmarks = bookmarkSelectors.all(state)
  const total = state.bookmarks.total

  return {
    bookmarks,
    total,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    load: start => dispatch(bookmarkActions.load({ start, })),
    save: bookmark => dispatch(bookmarkActions.save(bookmark)),
    remove: bookmark => dispatch(bookmarkActions.remove(bookmark)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Bookmarks)
