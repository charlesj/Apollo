import React from "react";

import "./FlexRow.css";

function FlexRow(props) {
  const classNames = "flexRow " + props.className;
  return <div className={classNames}>{props.children}</div>;
}

export default FlexRow;
