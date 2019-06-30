import React from 'react'
import PropTypes from 'prop-types'
import FontAwesome from 'react-fontawesome'

import { TextButton, Tag, } from '../_controls'

function Bookmark(props) {
  const {
    bookmark: { createdAtDisplay, link, title, tags, description, },
    editBookmark,
    deleteBookmark,
  } = props
  return (
    <div className="bookmark">
      <FontAwesome name="bookmark-o" />
      {createdAtDisplay}{' '}
      <a href={link} target="_blank">
        {title}
      </a>
      {description && <div className="description">{description}</div>}
      <div className="tags">
        {tags &&
          tags.map((t, i) => {
            return <Tag key={i} name={t} />
          })}
      </div>
      <div className="bookmarkCommands">
        <TextButton onClick={() => editBookmark(props.bookmark)}>
          <FontAwesome name="pencil" />Edit
        </TextButton>
        <TextButton onClick={() => deleteBookmark(props.bookmark)}>
          <FontAwesome name="remove" />Delete
        </TextButton>
      </div>
    </div>
  )
}

Bookmark.propTypes = {
  bookmark: PropTypes.object.isRequired,
  editBookmark: PropTypes.func.isRequired,
  deleteBookmark: PropTypes.func.isRequired,
}

export default Bookmark
