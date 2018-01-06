import React from "react";
import PropTypes from "prop-types";
import FontAwesome from "react-fontawesome";
import { Button, Container, FlexRow } from "../_controls";
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
      <Container>
        <FlexRow>
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
        </FlexRow>
        <Button onClick={() => loadBookmarks(bookmarks.length)} primary>
          <FontAwesome name="chevron-circle-right" /> Load More
        </Button>
      </Container>
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
