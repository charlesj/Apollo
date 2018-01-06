import React, { Component } from "react";
import FontAwesome from "react-fontawesome";
import apolloServer from "../../services/apolloServer";
import { TextButton } from "../_controls";

import "./Boards.css";

function BoardMenu(props) {
  return (
    <div>
      <TextButton onClick={props.updateBoardName}>rename board</TextButton>
      <TextButton onClick={props.moveLeft}>move left</TextButton>
      <TextButton onClick={props.moveRight}>move right</TextButton>
      <TextButton onClick={props.deleteBoard}>delete board</TextButton>
    </div>
  );
}

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
    this.toggleEdit = this.toggleEdit.bind(this);
    this.toggleCompleted = this.toggleCompleted.bind(this);
    this.updateItem = this.updateItem.bind(this);
  }

  toggleEdit() {
    this.setState({
      editMode: !this.state.editMode
    });
  }

  toggleCompleted() {
    var date = new Date();
    if (this.props.item.completed_at !== null) {
      date = null;
    }
    return this.props.updateItem(
      this.props.item.id,
      this.props.item.title,
      this.props.item.link,
      this.props.item.description,
      date
    );
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  updateItem() {
    return this.props
      .updateItem(
        this.props.item.id,
        this.state.newItemTitle,
        this.state.newItemLink,
        this.state.newItemDescription,
        this.props.item.completed_at
      )
      .then(() => {
        this.setState({
          editMode: false
        });
      });
  }

  render() {
    if (this.state.editMode) {
      return (
        <div className="pt-card">
          <input
            id="itemTitle"
            placeholder="title"
            onChange={this.handleChange}
            name="newItemTitle"
            value={this.state.newItemTitle}
          />
          <input
            id="itemLink"
            placeholder="link"
            onChange={this.handleChange}
            name="newItemLink"
            value={this.state.newItemLink}
          />
          <div>
            <textarea
              className="pt-input"
              placeholder="description"
              onChange={this.handleChange}
              name="newItemDescription"
              value={this.state.newItemDescription}
            />
          </div>
          <TextButton onClick={this.toggleEdit}>Cancel</TextButton>
          <TextButton onClick={this.updateItem}>updateItem</TextButton>
        </div>
      );
    }

    return (
      <div className="boardItem">
        <div className="boardItemTitle">
          {this.props.item.title}
          {this.props.item.link.length > 0 && (
            <a href={this.props.item.link} target="_blank">
              <FontAwesome name="external-link" />
            </a>
          )}
        </div>
        <div className="boardItemDescription">
          {this.props.item.description}
        </div>
        <div className="boardItemMenu">
          <TextButton onClick={this.toggleEdit}>edit</TextButton>
          <TextButton onClick={this.toggleCompleted}>complete</TextButton>
          <TextButton onClick={this.props.deleteItem}>delete</TextButton>
        </div>
      </div>
    );
  }
}

class Board extends Component {
  constructor(props) {
    super(props);
    this.state = {
      items: [],
      showMenu: false,
      showCompleted: false
    };

    this.addItem = this.addItem.bind(this);
    this.deleteItem = this.deleteItem.bind(this);
    this.loadItems = this.loadItems.bind(this);
    this.toggleCompleted = this.toggleCompleted.bind(this);
    this.toggleMenu = this.toggleMenu.bind(this);
    this.updateBoardName = this.updateBoardName.bind(this);
    this.updateItem = this.updateItem.bind(this);
  }

  componentDidMount() {
    this.loadItems();
  }

  loadItems() {
    return apolloServer
      .invoke("getBoardItems", {
        board_id: this.props.board.id
      })
      .then(items => {
        this.setState({
          items
        });
      });
  }

  toggleMenu() {
    this.setState({
      showMenu: !this.state.showMenu
    });
  }

  updateBoardName() {
    var name = prompt("Enter New Name", "");
    if (name !== null && name !== "") {
      this.props.updateBoard(
        this.props.board.id,
        name,
        this.props.board.list_order
      );
    }
  }

  addItem() {
    return apolloServer
      .invoke("addBoardItem", {
        board_id: this.props.board.id,
        title: "new item",
        link: "",
        description: ""
      })
      .then(() => {
        return this.loadItems();
      });
  }

  deleteItem(id) {
    return apolloServer
      .invoke("deleteBoardItem", {
        id
      })
      .then(() => {
        return this.loadItems();
      });
  }

  updateItem(id, title, link, description, completed_at) {
    return apolloServer
      .invoke("updateBoardItem", {
        id,
        board_id: this.props.board.id,
        title,
        link,
        description,
        completed_at
      })
      .then(() => {
        this.loadItems();
      });
  }

  toggleCompleted() {
    this.setState({
      showCompleted: !this.state.showCompleted
    });
  }

  render() {
    return (
      <div className="board">
        <div className="boardTitle">
          <span className="boardMenuToggle">
            <button className="textButton" onClick={this.toggleMenu}>
              e
            </button>
          </span>
          {this.props.board.title}
          {this.state.showMenu && (
            <BoardMenu
              updateBoardName={this.updateBoardName}
              moveLeft={this.props.moveBoard.bind(
                null,
                "left",
                this.props.board
              )}
              moveRight={this.props.moveBoard.bind(
                null,
                "right",
                this.props.board
              )}
              deleteBoard={this.props.deleteBoard.bind(
                null,
                this.props.board.id
              )}
            />
          )}
        </div>
        {this.state.items
          .filter(item => {
            if (item.completed_at == null) {
              return true;
            }
            return (item.completed_at !== null) === this.state.showCompleted;
          })
          .map(item => {
            return (
              <BoardItem
                key={item.id}
                item={item}
                deleteItem={this.deleteItem.bind(null, item.id)}
                updateItem={this.updateItem}
              />
            );
          })}
        <TextButton onClick={this.addItem}>New</TextButton>
        <TextButton onClick={this.toggleCompleted}>Show Completed</TextButton>
      </div>
    );
  }
}

class Boards extends Component {
  constructor(props) {
    super(props);

    this.state = {
      boards: []
    };

    this.addBoard = this.addBoard.bind(this);
    this.deleteBoard = this.deleteBoard.bind(this);
    this.loadBoards = this.loadBoards.bind(this);
    this.moveBoard = this.moveBoard.bind(this);
    this.updateBoard = this.updateBoard.bind(this);
  }

  componentDidMount() {
    this.loadBoards();
  }

  loadBoards() {
    return apolloServer.invoke("getBoards", {}).then(items => {
      this.setState({
        boards: items
      });
    });
  }

  addBoard() {
    return apolloServer
      .invoke("addBoard", {
        title: "new board",
        list_order: this.state.boards.length
      })
      .then(() => {
        this.loadBoards();
      });
  }

  updateBoard(id, title, list_order, supress_update = false) {
    return apolloServer
      .invoke("updateBoard", {
        id,
        title,
        list_order
      })
      .then(() => {
        !supress_update && this.loadBoards();
      });
  }

  deleteBoard(id) {
    return apolloServer
      .invoke("deleteBoard", {
        id
      })
      .then(() => {
        this.loadBoards();
      });
  }

  moveBoard(direction, board) {
    var curr_order = board.list_order;
    if (direction === "left" && board.list_order > 0) {
      var prevBoard = this.state.boards[board.list_order - 1];
      board.list_order = prevBoard.list_order;
      prevBoard.list_order = curr_order;
      this.updateBoard(
        prevBoard.id,
        prevBoard.title,
        prevBoard.list_order,
        true
      ).then(() => {
        this.updateBoard(board.id, board.title, board.list_order);
      });
    } else if (
      direction === "right" &&
      board.list_order < this.state.boards.length - 1
    ) {
      var nextBoard = this.state.boards[board.list_order + 1];
      board.list_order = nextBoard.list_order;
      nextBoard.list_order = curr_order;
      this.updateBoard(
        nextBoard.id,
        nextBoard.title,
        nextBoard.list_order,
        true
      ).then(() => {
        this.updateBoard(board.id, board.title, board.list_order);
      });
    }
  }

  render() {
    return (
      <div>
        <button className="textButton" onClick={this.addBoard}>
          Add New Board
        </button>
        <div className="boardContainer">
          {this.state.boards.map(board => {
            return (
              <Board
                updateBoard={this.updateBoard}
                deleteBoard={this.deleteBoard}
                moveBoard={this.moveBoard}
                board={board}
                key={board.id}
              />
            );
          })}
        </div>
      </div>
    );
  }
}

export default Boards;
