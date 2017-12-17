import React from "react";

import "./Button.css";

function Button(props) {
  const { onClick, children, primary } = props;
  let classes = "button";
  if (primary) {
    classes = classes + " button-primary";
  }
  return (
    <button className={classes} onClick={onClick}>
      {children}
    </button>
  );
}

export default Button;
