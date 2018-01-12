import React, { Component } from "react";
import { connect } from "react-redux";

import { directions } from "../../redux/enums";
import { boardActions } from "../../redux/actions";
import { boardSelectors } from "../../redux/selectors";
import { FlexRow, AddButton, Page } from "../_controls";
import Board from "./Board";

import "./Boards.css";

class Boards extends Component {
  componentDidMount() {
    this.loadBoards();
  }

  loadBoards() {
    const { load } = this.props;
    load();
  }

  render() {
    const {
      boards,
      moveBoard,
      saveBoard,
      removeBoard,
      nextListOrder
    } = this.props;

    return (
      <Page>
        <AddButton
          onClick={() =>
            saveBoard({ title: "new board", list_order: nextListOrder })
          }
          noun="Board"
        />
        <FlexRow>
          {boards.map((board, index) => {
            return (
              <Board
                updateBoard={board => saveBoard(board)}
                removeBoard={board => removeBoard(board)}
                moveLeft={() => moveBoard(directions.left, boards, index)}
                moveRight={() => moveBoard(directions.right, boards, index)}
                board={board}
                key={board.id}
              />
            );
          })}
        </FlexRow>
      </Page>
    );
  }
}

function mapStateToProps(state, props) {
  const boards = boardSelectors.all(state);
  const nextListOrder = boardSelectors.nextListOrder(state);
  return {
    boards,
    nextListOrder
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    load: () => dispatch(boardActions.load()),
    saveBoard: board => dispatch(boardActions.saveBoard(board)),
    removeBoard: board => dispatch(boardActions.removeBoard(board)),
    moveBoard: (direction, boards, index) =>
      dispatch(boardActions.moveBoard({ direction, boards, index }))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Boards);
