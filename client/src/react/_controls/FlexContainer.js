import React from "react";
import ClassNames from "classnames";

function FlexContainer(props) {
  return (
    <div
      className={ClassNames({
        [props.className]: props.className,
        "flexContainer-grow": props.grow
      })}
    >
      {props.children}
    </div>
  );
}

export default FlexContainer;
