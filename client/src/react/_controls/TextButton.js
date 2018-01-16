import React from "react";
import ClassNames from "classnames";
import Button from "./Button";

function TextButton(props) {
  const { className, ...rest } = props;
  return (
    <Button className={ClassNames("textButton", className)} {...rest}>
      {props.children}
    </Button>
  );
}

export default TextButton;
