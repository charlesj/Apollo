import React from 'react'
import PropTypes from 'prop-types'
import './Card.css'

function Card(props) {
  const { title, content, } = props
  return (
    <div className="card">
      <div className="cardContent">{content}</div>
      <div className="cardTitle">{title}</div>
    </div>
  )
}

Card.propTypes = {
  title: PropTypes.string,
  content: PropTypes.any,
}

export default Card
