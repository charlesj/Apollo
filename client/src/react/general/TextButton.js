import React from "react";

import "./TextButton.css";

function TextButton(props) {
  return (
    <button className="textButton" onClick={props.onClick}>
      {props.children}
    </button>
  );
}

export default TextButton;
