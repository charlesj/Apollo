import React from "react";
import ClassNames from "classnames";
import "./FlexRow.css";

function FlexRow(props) {
  return (
    <div
      className={ClassNames({
        flexRow: true,
        "flexRow-wrap": props.wrap,
        [props.className]: props.className
      })}
    >
      {props.children}
    </div>
  );
}

export default FlexRow;
