import React from 'react'
import PropTypes from 'prop-types'
import './Page.css'

function Page(props) {
  return <div className="page">{props.children}</div>
}

Page.propTypes = {
  children: PropTypes.any,
}

export default Page
