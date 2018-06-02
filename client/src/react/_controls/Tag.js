import React from 'react'
import PropTypes from 'prop-types'

import './Tag.css'

function Tag(props) {
  const { name, } = props
  return <div className="tag">{name}</div>
}

Tag.propTypes = {
  name: PropTypes.string.isRequired,
}

export default Tag
