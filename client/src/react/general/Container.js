import React from "react";

import "./Container.css";

function Container(props) {
  const { children, grow } = props;
  let classes = "container";
  if (grow) {
    classes = classes + " container-grow";
  }
  return <div className={classes}>{children}</div>;
}

export default Container;
