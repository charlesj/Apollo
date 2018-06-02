import React, { Component, } from 'react'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'
import './SelectList.css'

class SelectList extends Component {
  constructor(props) {
    super(props)
    this.state = {
      selectedItem: null,
    }
  }

  selectItem(item) {
    this.props.onSelectItem(item)
    this.setState({
      selectedItem: item,
    })
  }

  render() {
    const { items, labelField, } = this.props
    const { selectedItem, } = this.state
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
              onClick={() => {
                this.selectItem(item)
              }}
            >
              {item[labelField]}
            </div>
          )
        })}
      </div>
    )
  }
}

SelectList.propTypes = {
  items: PropTypes.array,
  labelField: PropTypes.string,
  onSelectItem: PropTypes.func.isRequired,
}

export default SelectList
