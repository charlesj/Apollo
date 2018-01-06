import React from "react";

import "./Button.css";

function Button(props) {
  const { onClick, children, primary, type } = props;
  let classes = "button";
  if (primary) {
    classes = classes + " button-primary";
  }
  return (
    <button className={classes} type={type} onClick={onClick}>
      {children}
    </button>
  );
}

export default Button;
