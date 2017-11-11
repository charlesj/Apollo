import React from 'react';
import apolloServer from '../../services/apollo-server';

function BoardMenu(props) {
  return (<div>
    <button className="textButton" onClick={props.updateBoardName}>rename board</button>
    <button className="textButton" onClick={props.moveLeft}>move left</button>
    <button className="textButton" onClick={props.moveRight}>move right</button>
    <button className="textButton" onClick={props.deleteBoard}>delete board</button>
  </div>)
}

class Board extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      items: [],
      showMenu: false
    };

    this.loadItems = this.loadItems.bind(this);
    this.toggleMenu = this.toggleMenu.bind(this);
    this.updateBoardName = this.updateBoardName.bind(this);
  }

  componentDidMount() {
    this.loadItems();
  }

  loadItems() {
    apolloServer.invoke("getBoardItems", {
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
      this.props.updateBoard(this.props.board.id, name, this.props.board.list_order);
    }
  }

  render() {
    return <div className="board">
      <div className="boardTitle">
        <span className="boardMenuToggle">
          <button className="textButton" onClick={this.toggleMenu}>e</button>
        </span>
        {this.props.board.title}
        {this.state.showMenu && <BoardMenu
      updateBoardName={this.updateBoardName}
      moveLeft={this.props.moveBoard.bind(null, "left", this.props.board)}
      moveRight={this.props.moveBoard.bind(null, "right", this.props.board)}
      deleteBoard={this.props.deleteBoard.bind(null, this.props.board.id)}
      />}
      </div>
      {this.state.items.map(item => {
        return <div className="boardItem" key={item.id}>{item.title}</div>
      })}
    </div>
  }
}

class Boards extends React.Component {
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
    return apolloServer.invoke("getBoards", {})
      .then(items => {
        this.setState({
          boards: items
        });
      });
  }

  addBoard() {
    return apolloServer.invoke("addBoard", {
      title: "new board",
      list_order: this.state.boards.length
    }).then(() => {
      this.loadBoards();
    });
  }

  updateBoard(id, title, list_order, supress_update = false) {
    return apolloServer.invoke("updateBoard", {
      id,
      title,
      list_order
    }).then(() => {
      !supress_update && this.loadBoards();
    });
  }

  deleteBoard(id) {
    return apolloServer.invoke("deleteBoard", {
      id
    }).then(() => {
      this.loadBoards();
    });
  }

  moveBoard(direction, board) {
    var curr_order = board.list_order;
    if (direction === "left" && board.list_order > 0) {
      var prevBoard = this.state.boards[board.list_order - 1];
      board.list_order = prevBoard.list_order;
      prevBoard.list_order = curr_order;
      this.updateBoard(prevBoard.id, prevBoard.title, prevBoard.list_order, true)
        .then(() => {
          this.updateBoard(board.id, board.title, board.list_order)
        });
    } else if (direction === "right" && board.list_order < this.state.boards.length - 1) {
      var nextBoard = this.state.boards[board.list_order + 1];
      board.list_order = nextBoard.list_order;
      nextBoard.list_order = curr_order;
      this.updateBoard(nextBoard.id, nextBoard.title, nextBoard.list_order, true)
        .then(() => {
          this.updateBoard(board.id, board.title, board.list_order)
        });
    }
  }

  render() {
    return <div><button className="textButton" onClick={this.addBoard}>Add New Board</button>
      <div className="boardContainer">
      { this.state.boards.map(board => {
        return <Board
          updateBoard={this.updateBoard}
          deleteBoard={this.deleteBoard}
          moveBoard={this.moveBoard}
          board={board}
          key={board.id} />
      })}
    </div>
  </div>
  }
}

module.exports = Boards;
