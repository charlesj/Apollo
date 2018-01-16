import React from "react";

function FlexContainer(props) {
  return <div className={props.className}>{props.children}</div>;
}

export default FlexContainer;
