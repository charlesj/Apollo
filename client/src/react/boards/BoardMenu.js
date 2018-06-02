import React from 'react'
import FontAwesome from 'react-fontawesome'
import PropTypes from 'prop-types'
import { TextButton, } from '../_controls'

function BoardMenu({ updateBoardName, moveLeft, moveRight, deleteBoard, }) {
  return (
    <div className="boardMenu">
      <TextButton onClick={updateBoardName}>
        <FontAwesome name="pencil-square-o" />
      </TextButton>
      <TextButton onClick={moveLeft}>
        <FontAwesome name="arrow-left" />
      </TextButton>
      <TextButton onClick={moveRight}>
        <FontAwesome name="arrow-right" />
      </TextButton>
      <TextButton onClick={deleteBoard}>
        <FontAwesome name="trash" />
      </TextButton>
    </div>
  )
}

BoardMenu.propTypes = {
  updateBoardName: PropTypes.func.isRequired,
  moveLeft: PropTypes.func.isRequired,
  moveRight: PropTypes.func.isRequired,
  deleteBoard: PropTypes.func.isRequired,
}

export default BoardMenu
