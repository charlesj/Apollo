import React from "react";
import Button from "./Button";

function TextButton(props) {
  return (
    <Button className="textButton" onClick={props.onClick}>
      {props.children}
    </Button>
  );
}

export default TextButton;
