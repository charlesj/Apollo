import React from 'react'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'
import './SelectList.css'

function SelectList(props) {
  const {
    items,
    labelField,
    selectedItem,
    onSelectItem,
  } = props

  return (
    <div className="selectList">
      {items.map(item => {
        return (
          <div
            className={ClassNames({
              selectListItem: true,
              'selectListItem-selected':
                  selectedItem && item.id === selectedItem.id,
            })}
            key={item.id}
            onClick={() => onSelectItem(item)}
          >
            {item[labelField]}
          </div>
        )
      })}
    </div>
  )
}


SelectList.propTypes = {
  items: PropTypes.array,
  labelField: PropTypes.string,
  onSelectItem: PropTypes.func.isRequired,
  selectedItem: PropTypes.object,
}

export default SelectList
