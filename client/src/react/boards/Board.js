import React, { Component } from "react";
import { connect } from "react-redux";

import { boardActions } from "../../redux/actions";
import { boardSelectors } from "../../redux/selectors";
import { TextButton, AddButton } from "../_controls";
import BoardMenu from "./BoardMenu";
import BoardItem from "./BoardItem";

class Board extends Component {
  constructor(props) {
    super(props);
    this.state = {
      showCompleted: false
    };

    this.updateBoardName = this.updateBoardName.bind(this);
  }

  componentDidMount() {
    const { loadItems, board } = this.props;
    loadItems(board.id);
  }

  updateBoardName() {
    var title = prompt("Enter New Name", "");
    const { updateBoard, board } = this.props;
    if (title !== null && title !== "") {
      updateBoard({
        ...board,
        title
      });
    }
  }

  render() {
    const {
      board,
      incomplete_items,
      complete_items,
      removeBoard,
      moveLeft,
      moveRight,
      removeItem,
      saveItem
    } = this.props;
    const { showCompleted } = this.state;
    return (
      <div className="board">
        <div className="boardTitle">
          {this.props.board.title}

          <BoardMenu
            updateBoardName={this.updateBoardName}
            moveLeft={moveLeft}
            moveRight={moveRight}
            deleteBoard={() => removeBoard(board)}
          />
        </div>
        {incomplete_items.map(item => {
          return (
            <BoardItem
              key={item.id}
              item={item}
              deleteItem={() => removeItem(item)}
              updateItem={updated => saveItem(updated)}
            />
          );
        })}

        {showCompleted &&
          complete_items.map(item => {
            return (
              <BoardItem
                key={item.id}
                item={item}
                deleteItem={() => removeItem(item)}
                updateItem={updated => saveItem(updated)}
              />
            );
          })}
        <AddButton
          onClick={() =>
            saveItem({
              title: "new item",
              link: "",
              description: "",
              board_id: board.id
            })
          }
          noun="Item"
        />

        <TextButton
          onClick={() => this.setState({ showCompleted: !showCompleted })}
        >
          Toggle View Completed
        </TextButton>
      </div>
    );
  }
}

function mapStateToProps(state, props) {
  const incomplete_items = boardSelectors.incomplete_items(
    state,
    props.board.id
  );
  const complete_items = boardSelectors.complete_items(state, props.board.id);
  return {
    incomplete_items,
    complete_items
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    loadItems: board_id => dispatch(boardActions.loadItems(board_id)),
    saveItem: boardItem => dispatch(boardActions.saveItem(boardItem)),
    removeItem: boardItem => dispatch(boardActions.removeItem(boardItem))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Board);
