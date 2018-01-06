import React from "react";
import PropTypes from "prop-types";
import FontAwesome from "react-fontawesome";

import Bookmark from "./Bookmark";

class BookmarksDisplay extends React.Component {
  render() {
    const {
      bookmarks,
      loadBookmarks,
      editBookmark,
      deleteBookmark
    } = this.props;
    return (
      <div className="bookmarksContainer">
        {bookmarks.map(b => {
          return (
            <Bookmark
              key={b.id}
              bookmark={b}
              editBookmark={editBookmark}
              deleteBookmark={deleteBookmark}
            />
          );
        })}
        <button onClick={() => loadBookmarks(bookmarks.length)}>
          <FontAwesome name="chevron-circle-right" /> Load More
        </button>
      </div>
    );
  }
}

BookmarksDisplay.propTypes = {
  bookmarks: PropTypes.array.isRequired,
  loadBookmarks: PropTypes.func.isRequired,
  editBookmark: PropTypes.func.isRequired,
  deleteBookmark: PropTypes.func.isRequired
};

export default BookmarksDisplay;
