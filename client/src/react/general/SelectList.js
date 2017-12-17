import React, { Component } from "react";

import "./SelectList.css";

class SelectList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedItemId: null
    };
  }

  selectItem(item) {
    this.props.onSelectItem(item);
    this.setState({
      selectedItemId: item.id
    });
  }

  getClasses(item) {
    if (item.id === this.state.selectedItemId) {
      return "selectListItem selectListItem-selected";
    }
    return "selectListItem";
  }

  render() {
    const { items } = this.props;
    return (
      <div className="selectList">
        {items.map(item => {
          return (
            <div
              className={this.getClasses(item)}
              key={item.id}
              onClick={() => {
                this.selectItem(item);
              }}
            >
              {item.label}
            </div>
          );
        })}
      </div>
    );
  }
}

export default SelectList;
