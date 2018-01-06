import React from "react";
import PropTypes from "prop-types";
import FontAwesome from "react-fontawesome";

import { TextButton } from "../_controls";

function Bookmark(props) {
  const {
    bookmark: { createdAtDisplay, link, title, tags, description },
    editBookmark,
    deleteBookmark
  } = props;
  return (
    <div className="bookmark">
      <FontAwesome name="bookmark-o" />
      {createdAtDisplay}{" "}
      <a href={link} target="_blank">
        {title}
      </a>
      {description && <div className="description">{description}</div>}
      <div className="tags">
        {tags &&
          tags.map((t, i) => {
            return (
              <span key={i} className="tag">
                {t}
              </span>
            );
          })}
        <div className="bookmarkCommands">
          <TextButton onClick={() => editBookmark(props.bookmark)}>
            <FontAwesome name="pencil" />Edit
          </TextButton>
          <TextButton onClick={() => deleteBookmark(props.bookmark)}>
            <FontAwesome name="remove" />Delete
          </TextButton>
        </div>
      </div>
    </div>
  );
}

Bookmark.proptypes = {
  bookmark: PropTypes.object.isRequired
};

export default Bookmark;
