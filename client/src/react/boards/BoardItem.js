import React, { Component } from "react";
import PropTypes from "prop-types";
import FontAwesome from "react-fontawesome";

import { TextButton, SaveButton, CancelButton } from "../_controls";

class BoardItem extends Component {
  constructor(props) {
    super(props);
    this.state = {
      editMode: false,
      newItemTitle: props.item.title,
      newItemLink: props.item.link,
      newItemDescription: props.item.description
    };

    this.handleChange = this.handleChange.bind(this);
  }

  toggleCompleted() {
    const { updateItem, item } = this.props;

    if (item.completed_at) {
      delete item["completed_at"];
    } else {
      item.completed_at = new Date();
    }

    updateItem(item);
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  save() {
    const { updateItem, item } = this.props;
    const { newItemTitle, newItemLink, newItemDescription } = this.state;

    item.title = newItemTitle;
    item.link = newItemLink;
    item.description = newItemDescription;

    updateItem(item);

    this.setState({
      editMode: false
    });
  }

  render() {
    const { item, deleteItem } = this.props;
    const {
      editMode,
      newItemTitle,
      newItemLink,
      newItemDescription
    } = this.state;
    if (editMode) {
      return (
        <div className="boardItem boardItemForm">
          <input
            id="itemTitle"
            placeholder="title"
            onChange={this.handleChange}
            name="newItemTitle"
            value={newItemTitle}
          />
          <input
            id="itemLink"
            placeholder="link"
            onChange={this.handleChange}
            name="newItemLink"
            value={newItemLink}
          />
          <div>
            <textarea
              className="pt-input"
              placeholder="description"
              onChange={this.handleChange}
              name="newItemDescription"
              value={newItemDescription}
            />
          </div>
          <CancelButton
            onClick={() => this.setState({ editMode: !editMode })}
          />
          <SaveButton onClick={() => this.save()} primary />
        </div>
      );
    }

    return (
      <div className="boardItem">
        <div className="boardItemTitle">
          {item.title}
          {item.link.length > 5 && (
            <a href={item.link} target="_blank">
              link <FontAwesome name="external-link" />
            </a>
          )}
        </div>
        {item.description.length > 0 && (
          <div className="boardItemDescription">{item.description}</div>
        )}
        <div className="boardItemMenu">
          <TextButton onClick={() => this.setState({ editMode: !editMode })}>
            edit
          </TextButton>
          <TextButton onClick={() => this.toggleCompleted()}>
            complete
          </TextButton>
          <TextButton onClick={() => deleteItem()}>delete</TextButton>
        </div>
      </div>
    );
  }
}

BoardItem.propTypes = {
  item: PropTypes.object.isRequired,
  deleteItem: PropTypes.func.isRequired,
  updateItem: PropTypes.func.isRequired
};

export default BoardItem;
