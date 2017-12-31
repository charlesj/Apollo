import React from "react";

import "./Container.css";

function Container(props) {
  const { children, grow } = props;
  let { className } = props;
  className = className + " container";
  if (grow) {
    className = className + " container-grow";
  }

  return <div className={className}>{children}</div>;
}

export default Container;
