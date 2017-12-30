import React from "react";
import PropTypes from "prop-types";
import FontAwesome from "react-fontawesome";
import moment from "moment";

function SingleBookmark(props) {
  var createTime = moment(props.createdAt);
  return (
    <div className="bookmark">
      {" "}
      {createTime.calendar()} -
      <a href={props.link}>{props.title}</a>
      {props.description && (
        <div className="description">{props.description}</div>
      )}
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

SingleBookmark.propTypes = {
  createdAt: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  link: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired
};

class BookmarksDisplay extends React.Component {
  render() {
    return (
      <div>
        <div className="bookmarksDisplayHeader">
          Total Bookmarks: {this.props.totalBookmarks}
          <button
            className="pt-button pt-intent-success"
            onClick={this.props.refreshBookmarks}
          >
            <FontAwesome name="refresh" />
          </button>
        </div>
        {this.props.bookmarks.map(b => {
          return (
            <SingleBookmark
              createdAt={b.created_at}
              title={b.title}
              link={b.link}
              description={b.description}
              key={b.id}
              id={b.id}
              tags={b.tags}
            />
          );
        })}

        <button
          className="pt-button pt-intent-success buttonSpace"
          onClick={this.props.loadBookmarks}
        >
          Load More
        </button>
      </div>
    );
  }
}

export default BookmarksDisplay;
